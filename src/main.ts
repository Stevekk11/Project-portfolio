import net from 'node:net';
import fs from 'node:fs/promises';
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
import path from 'node:path';
import winston from 'winston'; // not cigarettes⚠️
import DailyRotateFile from 'winston-daily-rotate-file';

// Loading the config
const configPath = path.resolve('./app_config.json');
if (!existsSync(configPath)) {
    console.error("Chybí app_config.json!");
    process.exit(1);
}
const CONFIG = JSON.parse(readFileSync(configPath, 'utf-8'));

{
    const logDir = path.dirname(CONFIG.LOG_FILE);
    if (!existsSync(logDir)) mkdirSync(logDir, { recursive: true });

    if (!existsSync(CONFIG.LOG_FILE)) {
        try {
            writeFileSync(CONFIG.LOG_FILE, '');
        } catch { }
    }
}

// Make date pattern safe for filenames on Windows (':' not allowed) and avoid spaces
const rawDatePattern = (CONFIG.DATE_FORMAT || 'YYYY-MM-DD');
const safeDatePattern = rawDatePattern.replace(/:/g, '-').replace(/\s+/g, '_');

// Build a filename pattern for rotation that places %DATE% before the extension
let rotateFilename: string;
if (CONFIG.LOG_FILE.includes('%DATE%')) {
    rotateFilename = CONFIG.LOG_FILE;
} else {
    const ext = path.extname(CONFIG.LOG_FILE) || '.log';
    const base = CONFIG.LOG_FILE.slice(0, -ext.length);
    rotateFilename = `${base}-%DATE%${ext}`;
}

// Logging configuration
const logger = winston.createLogger({
    level: 'info',
    format: winston.format.combine(
        winston.format.timestamp(),
        winston.format.printf(({ timestamp, level, message }) => `[${timestamp}] ${level.toUpperCase()}: ${message}`)
    ),
    transports: [
        new winston.transports.Console(),
        new DailyRotateFile({
            filename: rotateFilename,
            datePattern: safeDatePattern,
            maxSize: CONFIG.LOG_MAX_SIZE,
            maxFiles: CONFIG.LOG_MAX_FILES,
            zippedArchive: true
        })
    ]
});


function withTimeout<T>(promise: Promise<T>, ms: number): Promise<T> {
    return Promise.race([
        promise,
        new Promise<never>((_, reject) => setTimeout(() => reject(new Error('TIMEOUT')), ms))
    ]);
}

async function startServer() {
    if (!existsSync(CONFIG.ACCOUNTS_DIR)) mkdirSync(CONFIG.ACCOUNTS_DIR);

    const server = net.createServer((socket) => {
        const remoteInfo = `${socket.remoteAddress}:${socket.remotePort}`;
        logger.info(`Připojen klient: ${remoteInfo}`);

        socket.setTimeout(CONFIG.CLIENT_IDLE_TIMEOUT);
        socket.on('timeout', () => {
            logger.warn(`Klient ${remoteInfo} odpojen pro neaktivitu.`);
            socket.end();
        });

        socket.on('data', async (data) => {
            const input = data.toString().trim();
            if (!input) return;

            const [command, ...args] = input.split(/\s+/);
            const bankCode = socket.localAddress.replace('::ffff:', '');
            const getPath = (acc: string) => path.join(CONFIG.ACCOUNTS_DIR, `${acc}.txt`);

            try {
                await withTimeout((async () => {
                    switch (command) {
                        case 'BC': // Bank Code
                            socket.write(`BC ${bankCode}\r\n`);
                            break;

                        case 'AC': // Account Create
                            let accNum: number;
                            let fPath: string;
                            do {
                                accNum = Math.floor(Math.random() * 90000) + 10000;
                                fPath = getPath(accNum.toString());
                            } while (existsSync(fPath));
                            await fs.writeFile(fPath, "0");
                            socket.write(`AC ${accNum}/${bankCode}\r\n`);
                            logger.info(`Vytvořen účet ${accNum} pro ${remoteInfo}`);
                            break;

                        case 'AD': // Deposit
                        case 'AW': { // Withdrawal
                            const [target, amountStr] = args;
                            const [acc, ip] = (target || "").split('/');
                            const f = getPath(acc);

                            if (ip !== bankCode || !existsSync(f) || !/^\d+$/.test(amountStr)) {
                                socket.write(`ER Špatný formát nebo účet neexistuje.\r\n`);
                            } else {
                                const balance = BigInt(await fs.readFile(f, 'utf8'));
                                const amount = BigInt(amountStr);
                                let newBalance: bigint;

                                if (command === 'AD') {
                                    newBalance = balance + amount;
                                } else {
                                    if (balance < amount) throw new Error("LOW_FUNDS");
                                    newBalance = balance - amount;
                                }

                                await fs.writeFile(f, newBalance.toString());
                                socket.write(`${command}\n`);
                                logger.info(`${command === 'AD' ? 'Vklad' : 'Výběr'} na účtu ${acc}: ${amount}`);
                            }
                            break;
                        }

                        case 'AB': { // Balance
                            const [target] = args;
                            const acc = (target || "").split('/')[0];
                            const f = getPath(acc);
                            if (!existsSync(f)) {
                                socket.write(`ER Formát čísla účtu není správný.\n`);
                            } else {
                                const balance = await fs.readFile(f, 'utf8');
                                socket.write(`AB ${balance}\n`);
                            }
                            break;
                        }

                        case 'AR': { // Remove
                            const [target] = args;
                            const acc = (target || "").split('/')[0];
                            const f = getPath(acc);
                            if (existsSync(f)) {
                                const balance = await fs.readFile(f, 'utf8');
                                if (balance === "0") {
                                    await fs.unlink(f);
                                    socket.write(`AR\n`);
                                    logger.info(`Účet ${acc} smazán.`);
                                } else {
                                    socket.write(`ER Nelze smazat bankovní účet na kterém jsou finance.\n`);
                                }
                            } else {
                                socket.write(`ER Účet neexistuje.\n`);
                            }
                            break;
                        }

                        case 'BA': { // Bank Amount
                            const files = await fs.readdir(CONFIG.ACCOUNTS_DIR);
                            let total = 0n;
                            for (const file of files) {
                                total += BigInt(await fs.readFile(path.join(CONFIG.ACCOUNTS_DIR, file), 'utf8'));
                            }
                            socket.write(`BA ${total.toString()}\n`);
                            break;
                        }

                        case 'BN': { // Bank Number of clients
                            const files = await fs.readdir(CONFIG.ACCOUNTS_DIR);
                            socket.write(`BN ${files.length}\n`);
                            break;
                        }

                        default:
                            socket.write(`ER Neznámý příkaz\n`);
                    }
                })(), CONFIG.RESPONSE_TIMEOUT);

            } catch (err: any) {
                let errMsg = "ER Chyba na serveru";
                if (err.message === 'TIMEOUT') errMsg = "ER Operace trvala příliš dlouho!";
                if (err.message === 'LOW_FUNDS') errMsg = "ER Není dostatek finančních prostředků!";

                socket.write(`${errMsg}\n`);
                logger.error(`Chyba (${remoteInfo}): ${err.message}`);
            }
        });

        socket.on('error', (err) => logger.error(`Socket error: ${err.message}`));
    });

    server.listen(CONFIG.PORT, CONFIG.HOST, () => {
        logger.info(`bankovní SERVER spuštěn na ${CONFIG.HOST}:${CONFIG.PORT}`);
    });
}

startServer().catch(err => logger.error(`FATAL: ${err.message}`));