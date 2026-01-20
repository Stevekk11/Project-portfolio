namespace Cviceni_3_4;

public static class CommandFactory
{
    public static ICommand CreateCommand(string commandText, StreamReader sr, StreamWriter sw, TcpServer server)
    {
        string[] parts = commandText.Split(' ');
        string commandName = parts[0].ToLower();

        switch (commandName)
        {
            case "who":
                return new WhoCommand();
            case "uptime":
                return new UptimeCommand();
            case "stats":
                return new StatsCommand();
            case "last":
                if (parts.Length > 1)
                    return new LastCommand(parts[1]);
                return null; // Invalid input for last
            case "exit":
                return new ExitCommand();
            default:
                return null; // Unknown command
        }
    }
}
