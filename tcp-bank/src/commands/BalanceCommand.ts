import fs from 'node:fs/promises';
import { Command, CommandContext } from './types.js';
import { findAccountFile, proxyCommand } from './helpers.js';

/**
 * AB: Získání zůstatku s vyhledáváním podle prefixu.
 */
export class BalanceCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, args, bankCode, CONFIG, logger } = ctx;
        const [target] = args;
        const [acc, ip] = (target || "").split('/');

        if (ip && ip !== bankCode) {
            try {
                const response = await proxyCommand(ip, CONFIG.PORT, `AB ${target}`, CONFIG.RESPONSE_TIMEOUT);
                socket.write(`${response}\r\n`);
            } catch (err: any) {
                socket.write(`ER Chyba při komunikaci s cizí bankou: ${err.message}\r\n`);
                logger.error(`Proxy error (${ip}): ${err.message}`);
            }
            return;
        }

        const f = await findAccountFile(CONFIG.ACCOUNTS_DIR, acc);
        if (!f) {
            socket.write(`ER Účet neexistuje.\r\n`);
        } else {
            const balance = await fs.readFile(f, 'utf8');
            socket.write(`AB ${balance}\r\n`);
        }
    }
}
