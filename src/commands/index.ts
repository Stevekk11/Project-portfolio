import { Command } from './types.js';
import { BankCodeCommand } from './BankCodeCommand.js';
import { AccountCreateCommand } from './AccountCreateCommand.js';
import { TransactionCommand } from './TransactionCommand.js';
import { BalanceCommand } from './BalanceCommand.js';
import { RemoveCommand } from './RemoveCommand.js';
import { BankAmountCommand } from './BankAmountCommand.js';
import { BankClientsCommand } from './BankClientsCommand.js';
import { ExitCommand } from './ExitCommand.js';

export * from './types.js';
export * from './helpers.js';

export const commandRegistry: Map<string, Command> = new Map([
    ['BC', new BankCodeCommand()],
    ['AC', new AccountCreateCommand()],
    ['AD', new TransactionCommand('AD')],
    ['AW', new TransactionCommand('AW')],
    ['AB', new BalanceCommand()],
    ['AR', new RemoveCommand()],
    ['BA', new BankAmountCommand()],
    ['BN', new BankClientsCommand()],
    ['exit', new ExitCommand()],
]);
