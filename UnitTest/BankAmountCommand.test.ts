import { describe, it, expect, vi, beforeEach } from 'vitest';
import { BankAmountCommand } from '../src/commands/BankAmountCommand.js';
import { CommandContext } from '../src/commands/types.js';
import fs from 'node:fs/promises';

vi.mock('node:fs/promises', () => ({
    default: {
        readdir: vi.fn(),
        readFile: vi.fn(),
    }
}));

describe('BankAmountCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should calculate total amount of all accounts', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (fs.readdir as any).mockResolvedValue(['acc1.txt', 'acc2.txt']);
        (fs.readFile as any)
            .mockResolvedValueOnce('1000')
            .mockResolvedValueOnce('2000');

        const command = new BankAmountCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('BA 3000\r\n');
    });

    it('should return 0 if no accounts exist', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (fs.readdir as any).mockResolvedValue([]);

        const command = new BankAmountCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('BA 0\r\n');
    });
});
