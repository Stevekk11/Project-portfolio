import { Command, CommandContext } from './types.js';

export class ExitCommand implements Command {
    async execute(ctx: CommandContext): Promise<void> {
        const { socket, logger, remoteInfo } = ctx;
        socket.write(`OK Goodbye\r\n`);
        logger.info(`Klient ${remoteInfo} ukončil spojení.`);
        socket.end();
    }
}
