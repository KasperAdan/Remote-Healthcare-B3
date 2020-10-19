using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        private static TcpListener Listener;
        private static List<Client> Clients = new List<Client>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Server!");
            DoctorPasswordData.Init();
            AllClients.Init();
            Listener = new TcpListener(IPAddress.Any, 15243);
            Listener.Start();
            Listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);

            Console.ReadLine();

            while (true)
            {

            }
        }

        private static void OnConnect(IAsyncResult ar)
        {
            var tcpClient = Listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            //check if the client already excists
            Clients.Add(new Client(tcpClient));
            Listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        internal static void Broadcast(string packet)
        {
            foreach (var client in Clients)
            {
                client.Write(packet);
            }
        }

        internal static void Disconnect(Client client)
        {
            Clients.Remove(client);
            client.Disconnect();
            Console.WriteLine("Client disconnected");
        }

        internal static void SendToUser(string user, string packet)
        {
            foreach (var client in Clients.Where(c => c.UserName == user))
            {
                client.Write(packet);
            }
        }
    }
}
