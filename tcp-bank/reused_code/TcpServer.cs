using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Concurrent;

namespace Cviceni_3_4
{
    public class TcpServer
    {
        private TcpListener listener;
        private bool isRunning;
        private ConcurrentDictionary<string, int> errorCounts; // Dictionary to track error counts by IP
        private HashSet<string> loggedInUsers;
        private Dictionary<string, DateTime> userLoginHistory;
        private int totalUsers;
        private int failedLogins;
        private int commandsProcessed;
        private Dictionary<string, string> ipToUsernameMap;

        public DateTime StartTime { get; private set; }

        public TcpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            isRunning = true;
            errorCounts = new ConcurrentDictionary<string, int>(); // Initialize the errorCounts dictionary
            loggedInUsers = new HashSet<string>();
            userLoginHistory = new Dictionary<string, DateTime>();
            totalUsers = 0;
            failedLogins = 0;
            commandsProcessed = 0;
            StartTime = DateTime.Now;
            listener.Start();
            ipToUsernameMap = new Dictionary<string, string>();
            ServerLoop();
        }

        private void ServerLoop()
        {
            Console.WriteLine("Server byl spusten");
            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread thread = new Thread(new ParameterizedThreadStart(ClientLoop));
                thread.Start(client);
            }
        }

        private void ClientLoop(object? o)
{
    try
    {
        if (o == null)
        {
            return;
        }

        TcpClient client = (TcpClient)o;
        string clientIp =
            ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(); // Get client IP address

        // Check if the IP has already failed 3 times
        if (errorCounts.ContainsKey(clientIp) && errorCounts[clientIp] >= 3)
        {
            Console.WriteLine($"Connection from {clientIp} rejected due to too many failed login attempts.");
            client.Close();
            return; // Reject the connection
        }

        StreamReader sr = new StreamReader(client.GetStream(), Encoding.UTF8);
        StreamWriter sw = new StreamWriter(client.GetStream(), Encoding.UTF8);

        var credentials = File.ReadAllLines("D:\\TestVlakna\\Cviceni-3-4\\credentials.txt")
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

        sw.WriteLine("Login to the server:");
        sw.WriteLine("Username:");
        sw.Flush();
        string? username = sr.ReadLine();
        sw.WriteLine("Password:");
        sw.Flush();
        string? passwordInput = sr.ReadLine();

        if (credentials.ContainsKey(username) && credentials[username] == passwordInput)
        {
            sw.WriteLine("Login successful");
            sw.Flush();
            errorCounts[clientIp] = 0; // Reset error count on successful login

            // Increment the totalUsers counter after successful login
            totalUsers++;

            // Record the user's login time
            ipToUsernameMap[clientIp] = username;
            userLoginHistory[username] = DateTime.Now;
            loggedInUsers.Add(username);

            // Loop to accept commands continuously
            while (isRunning)
            {
                // Read the next command from the client
                string cmd = sr.ReadLine();
                if (cmd == null) break;  // If client disconnects, break the loop

                ICommand command = CommandFactory.CreateCommand(cmd, sr, sw, this);
                if (command != null)
                {
                    command.Execute(client, sr, sw, this);
                    commandsProcessed++; // Increment commandsProcessed counter after command execution
                }
                else
                {
                    sw.WriteLine("Unknown command.");
                    sw.Flush();
                }
            }

            // Client has disconnected or finished commands, so close the connection
            client.Close();
        }
        else
        {
            // Increment error count for this IP address
            errorCounts.AddOrUpdate(clientIp, 1, (key, oldValue) => oldValue + 1);
            sw.WriteLine("Invalid credentials. Try again.");
            sw.Flush();

            // Increment failed logins on incorrect attempt
            failedLogins++;

            // If error count reaches 3, block future connections from this IP
            if (errorCounts[clientIp] >= 3)
            {
                Console.WriteLine(
                    $"Connection from {clientIp} has been blocked due to too many failed login attempts.");
                client.Close();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


        // Method to get the list of logged-in users
        public List<string> GetLoggedInUsers()
        {
            return loggedInUsers.ToList();
        }

        // Method to log out the client
        public void LogoutClient(TcpClient client)
        {
            // Assuming you have a way to get the username based on the client
            string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
    
            // You would need a way to identify the username associated with the client
            string username = GetUsernameByIp(clientIp);  // This method should be implemented to retrieve the username
    
            if (username != null)
            {
                loggedInUsers.Remove(username);  // Remove the username from the loggedInUsers set
                Console.WriteLine($"User {username} has logged out.");
            }
            else
            {
                Console.WriteLine($"No user found for IP {clientIp}. Logout failed.");
            }
        }
        
        private string GetUsernameByIp(string clientIp)
        {
            if (ipToUsernameMap.ContainsKey(clientIp))
            {
                return ipToUsernameMap[clientIp];
            }
            return null;  // Return null if the IP is not associated with a username
        }


        // Method to retrieve stats for the server
        public Stats GetStats()
        {
            return new Stats
            {
                TotalUsers = totalUsers,
                FailedLogins = failedLogins,
                CommandsProcessed = commandsProcessed
            };
        }

        // Method to get the last login date for a user
        public DateTime? GetLastLogin(string username)
        {
            if (userLoginHistory.ContainsKey(username))
            {
                return userLoginHistory[username];
            }

            return null;
        }
    }

    // Stats class to hold the server's statistics
    public class Stats
    {
        public int TotalUsers { get; set; }
        public int FailedLogins { get; set; }
        public int CommandsProcessed { get; set; }
    }
}