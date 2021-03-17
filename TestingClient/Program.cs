using System;
using System.Text;
using SimpleTcp;
using System.Threading;

namespace TestingClient
{
    class Program
    {
        static private string IP = "127.0.0.1";
        static private string PORT = "9000";

        static SimpleTcpClient client = new SimpleTcpClient(Program.IP + ":" + PORT);

        static void Main(string[] args)
        {
            
            // set events
            client.Events.Connected += Connected;
            client.Events.Disconnected += Disconnected;
            client.Events.DataReceived += DataReceived;

            // let's go!
            client.Connect();

            while(true)
            {
                var message = Console.ReadLine();
                client.Send(message);

                Thread.Sleep(500);
            }
        }

        static void Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to Server");
        }

        static void Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected to Server");
        }

        static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("[" + e.IpPort + "] " + Encoding.UTF8.GetString(e.Data));
        }
    }
}
