import net from 'node:net';
import { Logger } from 'winston';
import { NetworkMonitor } from '../NetworkMonitor.js';

export interface CommandContext {
    socket: net.Socket;
    args: string[];
    bankCode: string;
    remoteInfo: string;
    logger: Logger;
    networkMonitor: NetworkMonitor;
    CONFIG: any;
}

export interface Command {
    execute(ctx: CommandContext): Promise<void>;
}
