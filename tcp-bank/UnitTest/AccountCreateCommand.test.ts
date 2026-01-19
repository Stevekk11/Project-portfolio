import { describe, it, expect, vi, beforeEach } from 'vitest';
import { AccountCreateCommand } from '../src/commands/AccountCreateCommand.js';
import { CommandContext } from '../src/commands/types.js';
import fs from 'node:fs/promises';
import { existsSync } from 'node:fs';

vi.mock('node:fs', () => ({
    existsSync: vi.fn(),
}));

vi.mock('node:fs/promises', () => ({
    default: {
        writeFile: vi.fn(),
    }
}));

describe('AccountCreateCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should create an account and write to socket', async () => {
        const socketMock = {
            remoteAddress: '192.168.1.1',
            write: vi.fn(),
        } as any;

        const loggerMock = {
            info: vi.fn(),
        } as any;

        const ctx: CommandContext = {
            socket: socketMock,
            bankCode: '1234',
            remoteInfo: '192.168.1.1:12345',
            logger: loggerMock,
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (existsSync as any).mockReturnValue(false);
        (fs.writeFile as any).mockResolvedValue(undefined);

        const command = new AccountCreateCommand();
        await command.execute(ctx);

        expect(fs.writeFile).toHaveBeenCalled();
        expect(socketMock.write).toHaveBeenCalledWith(expect.stringMatching(/^AC \d{5}\/1234\r\n$/));
        expect(loggerMock.info).toHaveBeenCalled();
    });
});
