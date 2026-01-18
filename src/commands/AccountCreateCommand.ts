import fs from 'node:fs/promises';
import { existsSync } from 'node:fs';
import path from 'node:path';
import { Command, CommandContext } from './types.js';

/**
 * AC: Vytvoří účet ve formátu <číslo>_<IP>.txt
 */
export class AccountCreateCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, bankCode, remoteInfo, logger, CONFIG } = ctx;

        // Získání čisté IP adresy klienta
        const clientIp = socket.remoteAddress?.replace('::ffff:', '') || '127.0.0.1';

        let accNum: number;
        let fPath: string;
        let fileName: string;

        do {
            accNum = Math.floor(Math.random() * 90000) + 10000;
            fileName = `${accNum}_${clientIp}.txt`;
            fPath = path.join(CONFIG.ACCOUNTS_DIR, fileName);
        } while (existsSync(fPath));

        await fs.writeFile(fPath, "0");
        socket.write(`AC ${accNum}/${bankCode}\r\n`);
        logger.info(`Vytvořen účet ${accNum} pro IP ${clientIp} (${remoteInfo})`);
    }
}
