using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server
{
    static class AllClients
    {
        public static Dictionary<string, Client> TotalClients;

        public static void Init()
        {
            TotalClients = new Dictionary<string, Client>();
        }

        public static void Add(string userName, Client client)
        {
            TotalClients.Add(userName, client);
        }

        public static void Remove(string username)
        {
            TotalClients.Remove(username);
        }
    }
}
