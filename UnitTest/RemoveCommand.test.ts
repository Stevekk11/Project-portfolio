import { describe, it, expect, vi, beforeEach } from 'vitest';
import { RemoveCommand } from '../src/commands/RemoveCommand.js';
import { CommandContext } from '../src/commands/types.js';
import * as helpers from '../src/commands/helpers.js';
import fs from 'node:fs/promises';

vi.mock('../src/commands/helpers.js', () => ({
    findAccountFile: vi.fn(),
}));

vi.mock('node:fs/promises', () => ({
    default: {
        readFile: vi.fn(),
        unlink: vi.fn(),
    }
}));

describe('RemoveCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should remove account if balance is 0 and IP matches', async () => {
        const socketMock = {
            remoteAddress: '192.168.1.1',
            write: vi.fn(),
        } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345'],
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_192.168.1.1.txt');
        (fs.readFile as any).mockResolvedValue('0');
        (fs.unlink as any).mockResolvedValue(undefined);

        const command = new RemoveCommand();
        await command.execute(ctx);

        expect(fs.unlink).toHaveBeenCalledWith('./accounts/12345_192.168.1.1.txt');
        expect(socketMock.write).toHaveBeenCalledWith('AR\r\n');
    });

    it('should not remove account if IP does not match', async () => {
        const socketMock = {
            remoteAddress: '192.168.1.2',
            write: vi.fn(),
        } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345'],
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_192.168.1.1.txt');

        const command = new RemoveCommand();
        await command.execute(ctx);

        expect(fs.unlink).not.toHaveBeenCalled();
        expect(socketMock.write).toHaveBeenCalledWith(expect.stringContaining('původní IP adresy'));
    });

    it('should not remove account if balance is not 0', async () => {
        const socketMock = {
            remoteAddress: '192.168.1.1',
            write: vi.fn(),
        } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345'],
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_192.168.1.1.txt');
        (fs.readFile as any).mockResolvedValue('100');

        const command = new RemoveCommand();
        await command.execute(ctx);

        expect(fs.unlink).not.toHaveBeenCalled();
        expect(socketMock.write).toHaveBeenCalledWith(expect.stringContaining('finance'));
    });
});
