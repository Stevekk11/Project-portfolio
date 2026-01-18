import net from 'node:net';
import fs from 'node:fs/promises';
import path from 'node:path';

/**
 * Pomocná funkce pro vyhledání souboru účtu podle čísla (prefixu).
 * Vrací celou cestu k souboru nebo null.
 */
export async function findAccountFile(accountsDir: string, accNum: string): Promise<string | null> {
    try {
        const files = await fs.readdir(accountsDir);
        const found = files.find(f => f.startsWith(`${accNum}_`));
        return found ? path.join(accountsDir, found) : null;
    } catch {
        return null;
    }
}

/**
 * Pomocná funkce pro přeposlání příkazu jiné bance (proxy).
 */
export async function proxyCommand(targetIp: string, targetPort: number, commandLine: string, timeoutMs: number): Promise<string> {
    return new Promise((resolve, reject) => {
        const socket = net.createConnection({ host: targetIp, port: targetPort }, () => {
            socket.write(commandLine + "\r\n");
        });

        socket.on('data', (data) => {
            resolve(data.toString().trim());
            socket.end();
        });

        socket.on('error', (err) => {
            reject(err);
        });

        socket.setTimeout(timeoutMs);
        socket.on('timeout', () => {
            socket.destroy();
            reject(new Error("TIMEOUT"));
        });
    });
}
