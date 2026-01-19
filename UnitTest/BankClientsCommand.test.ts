import { describe, it, expect, vi, beforeEach } from 'vitest';
import { BankClientsCommand } from '../src/commands/BankClientsCommand.js';
import { CommandContext } from '../src/commands/types.js';
import fs from 'node:fs/promises';

vi.mock('node:fs/promises', () => ({
    default: {
        readdir: vi.fn(),
    }
}));

describe('BankClientsCommand', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should calculate number of unique client IPs', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (fs.readdir as any).mockResolvedValue(['12345_192.168.1.1.txt', '67890_192.168.1.1.txt', '11111_192.168.1.2.txt']);

        const command = new BankClientsCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('BN 2\r\n');
    });

    it('should return 0 if no accounts exist', async () => {
        const socketMock = { write: vi.fn() } as any;
        const ctx: CommandContext = {
            socket: socketMock,
            CONFIG: { ACCOUNTS_DIR: './accounts' },
        } as any;

        (fs.readdir as any).mockResolvedValue([]);

        const command = new BankClientsCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('BN 0\r\n');
    });
});
