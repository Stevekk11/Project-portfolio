using System.Net.Sockets;

namespace Cviceni_3_4;

public class ExitCommand : ICommand
{
    public void Execute(TcpClient client, StreamReader sr, StreamWriter sw, TcpServer server)
    {
        server.LogoutClient(client);
        sw.WriteLine("You have been logged out.");
        sw.Flush();
        client.Close();
    }
}
