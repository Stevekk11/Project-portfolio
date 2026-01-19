import { describe, it, expect, vi, beforeEach } from 'vitest';
import { TransactionCommand } from '../src/commands/TransactionCommand.js';
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
        writeFile: vi.fn(),
    }
}));

describe('TransactionCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should deposit money to local account (AD)', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345', '500'],
            bankCode: '9999',
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_127.0.0.1.txt');
        (fs.readFile as any).mockResolvedValue('1000');
        (fs.writeFile as any).mockResolvedValue(undefined);

        const command = new TransactionCommand('AD');
        await command.execute(ctx);

        expect(fs.writeFile).toHaveBeenCalledWith('./accounts/12345_127.0.0.1.txt', '1500');
        expect(socketMock.write).toHaveBeenCalledWith('AD\r\n');
    });

    it('should withdraw money from local account (AW)', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345', '400'],
            bankCode: '9999',
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_127.0.0.1.txt');
        (fs.readFile as any).mockResolvedValue('1000');
        (fs.writeFile as any).mockResolvedValue(undefined);

        const command = new TransactionCommand('AW');
        await command.execute(ctx);

        expect(fs.writeFile).toHaveBeenCalledWith('./accounts/12345_127.0.0.1.txt', '600');
        expect(socketMock.write).toHaveBeenCalledWith('AW\r\n');
    });

    it('should throw error for insufficient funds (AW)', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            args: ['12345', '1500'],
            bankCode: '9999',
            CONFIG: { ACCOUNTS_DIR: './accounts' },
            logger: { info: vi.fn() } as any,
        } as any;

        (helpers.findAccountFile as any).mockResolvedValue('./accounts/12345_127.0.0.1.txt');
        (fs.readFile as any).mockResolvedValue('1000');

        const command = new TransactionCommand('AW');
        await expect(command.execute(ctx)).rejects.toThrow('LOW_FUNDS');
    });
});
