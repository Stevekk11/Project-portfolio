using System.Net.Sockets;

namespace Cviceni_3_4;

public class WhoCommand : ICommand
{
    public void Execute(TcpClient client, StreamReader sr, StreamWriter sw, TcpServer server)
    {
        var loggedInUsers = server.GetLoggedInUsers();
        if (loggedInUsers.Count > 0)
        {
            foreach (var user in loggedInUsers)
            {
                sw.WriteLine(user);
            }
        }
        else
        {
            sw.WriteLine("No users are currently logged in.");
        }
        sw.Flush();
    }
}
