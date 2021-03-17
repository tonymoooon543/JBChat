using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SimpleTcp;

namespace TcpServerTesting
{
    class Program
    {
        private const string PORT = "9000";
        private const string IP = "127.0.0.1";
        static SimpleTcpServer server = new SimpleTcpServer(IP + ":" + PORT);

        static int CheckCommand(string command)
        {
            switch(command)
            {
                case "com:clear":
                    Console.Clear();
                    return 1;
                case "com:end":
                    Console.WriteLine("Ending Server");
                    server.Dispose();
                    return 2;
                default:
                    Console.WriteLine("Unkown Command");
                    return 3;
            }
        }

        static void Main(string[] args)
        {
            // set events
            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.DataReceived += DataReceived;

            // let's go!
            server.Start();
            Console.WriteLine("Started server, on IP " + IP + " on port " + PORT);

            while (true)
            {
                var message = Console.ReadLine();
                if(CheckCommand(message) == 2)
                {
                    break;
                }

                Thread.Sleep(500);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("[" + e.IpPort + "] client connected");
        }

        static void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("[" + e.IpPort + "] client disconnected: " + e.Reason.ToString());
        }

        static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("[" + e.IpPort + "] says: " + Encoding.UTF8.GetString(e.Data));
            IEnumerable<string> clients = server.GetClients();
            foreach (string client in clients)
            {
                server.SendAsync(client, "says: " + Encoding.UTF8.GetString(e.Data));
            }
        }
    }
}
