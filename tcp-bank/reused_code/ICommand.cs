using System.Net.Sockets;

namespace Cviceni_3_4;

public interface ICommand
{
    void Execute(TcpClient client, StreamReader sr, StreamWriter sw, TcpServer server);
}
