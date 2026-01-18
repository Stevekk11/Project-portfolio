import fs from 'node:fs/promises';
import path from 'node:path';
import { Command, CommandContext } from './types.js';
import { findAccountFile } from './helpers.js';

/**
 * AR: Odstranění účtu (pouze pokud je zůstatek 0 a IP se shoduje).
 */
export class RemoveCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, args, CONFIG, logger } = ctx;
        const [target] = args;
        const acc = (target || "").split('/')[0];
        const clientIp = socket.remoteAddress?.replace('::ffff:', '') || '127.0.0.1';

        const f = await findAccountFile(CONFIG.ACCOUNTS_DIR, acc);

        if (f) {
            // Kontrola, zda účet patří této IP adrese (soubor obsahuje IP v názvu)
            const ownerIp = path.basename(f).replace('.txt', '').split('_')[1];

            if (ownerIp !== clientIp) {
                socket.write(`ER Účet může smazat pouze jeho zakladatel z původní IP adresy.\r\n`);
                return;
            }

            const balance = await fs.readFile(f, 'utf8');
            if (balance === "0") {
                await fs.unlink(f);
                socket.write(`AR\r\n`);
                logger.info(`Účet ${acc} smazán.`);
            } else {
                socket.write(`ER Nelze smazat bankovní účet na kterém jsou finance.\r\n`);
            }
        } else {
            socket.write(`ER Účet neexistuje.\r\n`);
        }
    }
}
