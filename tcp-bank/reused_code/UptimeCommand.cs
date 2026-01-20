using System.Net.Sockets;

namespace Cviceni_3_4;

public class UptimeCommand : ICommand
{
    public void Execute(TcpClient client, StreamReader sr, StreamWriter sw, TcpServer server)
    {
        TimeSpan uptime = DateTime.Now - server.StartTime;
        sw.WriteLine($"Server has been running for {uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes.");
        sw.Flush();
    }
}
