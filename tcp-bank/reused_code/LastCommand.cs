using System.Net.Sockets;

namespace Cviceni_3_4;

public class LastCommand : ICommand
{
    private string _username;

    public LastCommand(string username)
    {
        _username = username;
    }

    public void Execute(TcpClient client, StreamReader sr, StreamWriter sw, TcpServer server)
    {
        var lastLogin = server.GetLastLogin(_username);
        if (lastLogin != null)
        {
            sw.WriteLine($"{_username} was last logged in at {lastLogin}");
        }
        else
        {
            sw.WriteLine($"No login history found for user {_username}.");
        }
        sw.Flush();
    }
}
