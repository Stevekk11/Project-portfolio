import fs from 'node:fs/promises';
import path from 'node:path';
import { Command, CommandContext } from './types.js';

/**
 * BA: Celková suma všech financí v bance.
 */
export class BankAmountCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, CONFIG } = ctx;
        const files = await fs.readdir(CONFIG.ACCOUNTS_DIR);
        let total = 0n;
        for (const file of files) {
            total += BigInt(await fs.readFile(path.join(CONFIG.ACCOUNTS_DIR, file), 'utf8'));
        }
        socket.write(`BA ${total.toString()}\r\n`);
    }
}
