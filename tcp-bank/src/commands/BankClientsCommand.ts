import fs from 'node:fs/promises';
import { Command, CommandContext } from './types.js';

/**
 * BN: Počet unikátních IP adres (klientů).
 */
export class BankClientsCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, CONFIG } = ctx;
        const files = await fs.readdir(CONFIG.ACCOUNTS_DIR);

        const uniqueIps = new Set<string>();
        for (const file of files) {
            const parts = file.replace('.txt', '').split('_');
            if (parts.length > 1) {
                uniqueIps.add(parts[1]);
            }
        }
        socket.write(`BN ${uniqueIps.size}\r\n`);
    }
}
