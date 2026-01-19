import { describe, it, expect, vi, beforeEach } from 'vitest';
import { BalanceCommand } from '../src/commands/BalanceCommand.js';
import { CommandContext } from '../src/commands/types.js';
import * as helpers from '../src/commands/helpers.js';
import fs from 'node:fs/promises';

vi.mock('../src/commands/helpers.js', () => ({
    findAccountFile: vi.fn(),
    proxyCommand: vi.fn(),
}));

vi.mock('node:fs/promises', () => ({
    default: {
        readFile: vi.fn(),
    }
}));

describe('BalanceCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should return balance for local account', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345'],
            bankCode: '9999',
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { error: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_127.0.0.1.txt');
        (fs.readFile as any).mockResolvedValue('1000');

        const command = new BalanceCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('AB 1000\r\n');
    });

    it('should proxy command for remote account', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345/1111'],
            bankCode: '9999',
            CONFIG: { PORT: 65525, RESPONSE_TIMEOUT: 5000 },
            logger: { error: vi.fn() } as any,
        } as any;

        (helpers.proxyCommand as any).mockResolvedValue('AB 2000');

        const command = new BalanceCommand();
        await command.execute(ctx);

        expect(helpers.proxyCommand).toHaveBeenCalledWith('1111', 65525, 'AB 12345/1111', 5000);
        expect(socketMock.write).toHaveBeenCalledWith('AB 2000\r\n');
    });

    it('should return error if account does not exist', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345'],
            bankCode: '9999',
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue(null);

        const command = new BalanceCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('ER Účet neexistuje.\r\n');
    });
});
