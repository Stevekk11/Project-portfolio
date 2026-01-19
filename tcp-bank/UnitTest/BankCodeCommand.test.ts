import { describe, it, expect, vi } from 'vitest';
import { BankCodeCommand } from '../src/commands/BankCodeCommand.js';
import { CommandContext } from '../src/commands/types.js';

describe('BankCodeCommand', () => {
    it('should send the bank code to the socket', async () => {
        const socketMock = {
            write: vi.fn(),
        } as any;

        const ctx: CommandContext = {
            socket: socketMock,
            bankCode: '1234',
        } as any;

        const command = new BankCodeCommand();
        await command.execute(ctx);

        expect(socketMock.write).toHaveBeenCalledWith('BC 1234\r\n');
    });
});
