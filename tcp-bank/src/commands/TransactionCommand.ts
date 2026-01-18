import fs from 'node:fs/promises';
import { Command, CommandContext } from './types.js';
import { findAccountFile, proxyCommand } from './helpers.js';

/**
 * AD/AW: Transakce s podporou proxy a vyhledáváním souborů.
 */
export class TransactionCommand implements Command {
    constructor(private type: 'AD' | 'AW') {}

    async execute(ctx: CommandContext): Promise<void> {
        const { socket, args, bankCode, logger, CONFIG } = ctx;
        const [target, amountStr] = args;
        const [acc, ip] = (target || "").split('/');

        // Proxy logika pro cizí banky
        if (ip && ip !== bankCode) {
            try {
                const response = await proxyCommand(ip, CONFIG.PORT, `${this.type} ${target} ${amountStr}`, CONFIG.RESPONSE_TIMEOUT);
                socket.write(`${response}\r\n`);
            } catch (err: any) {
                socket.write(`ER Chyba při komunikaci s cizí bankou: ${err.message}\r\n`);
                logger.error(`Proxy error (${ip}): ${err.message}`);
            }
            return;
        }

        // Lokální zpracování - vyhledání souboru s IP v názvu
        const f = await findAccountFile(CONFIG.ACCOUNTS_DIR, acc);

        if (!f || !/^\d+$/.test(amountStr)) {
            socket.write(`ER Špatný formát nebo účet neexistuje.\r\n`);
        } else {
            const balance = BigInt(await fs.readFile(f, 'utf8'));
            const amount = BigInt(amountStr);
            let newBalance: bigint;

            if (this.type === 'AD') {
                newBalance = balance + amount;
            } else {
                if (balance < amount) throw new Error("LOW_FUNDS");
                newBalance = balance - amount;
            }

            await fs.writeFile(f, newBalance.toString());
            socket.write(`${this.type}\r\n`);
            logger.info(`${this.type === 'AD' ? 'Vklad' : 'Výběr'} na účtu ${acc}: ${amount}`);
        }
    }
}
