using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server
{
    static class AllClients
    {
        public static Dictionary<string, Client> totalClients;

        public static void init()
        {
            totalClients = new Dictionary<string, Client>();
        }

        public static void Add(string userName, Client client)
        {
            totalClients.Add(userName, client);
        }

        public static void Remove(string username)
        {
            totalClients.Remove(username);
        }
    }
}
