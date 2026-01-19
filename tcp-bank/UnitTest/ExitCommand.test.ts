import { describe, it, expect, vi } from 'vitest';
import { ExitCommand } from '../src/commands/ExitCommand.js';
import { CommandContext } from '../src/commands/types.js';

describe('ExitCommand', () => {
    it('should send goodbye message and close socket', async () => {
        const socketMock = {
            write: vi.fn(),
            end: vi.fn(),
        } as any;
        const loggerMock = {
            info: vi.fn(),
        } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            logger: loggerMock,
            remoteInfo: '127.0.0.1:12345',
        } as any;

        const command = new ExitCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('OK Goodbye\r\n');
        expect(loggerMock.info).toHaveBeenCalled();
        expect(socketMock.end).toHaveBeenCalled();
    });
});
