import { Command, CommandContext } from './types.js';

export class BankCodeCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        ctx.socket.write(`BC ${ctx.bankCode}\r\n`);
    }
}
