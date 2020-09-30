using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server
{
    static class AllClientData
    {
        public static Dictionary<string, ClientData> TotalClientData;

        public static void init()
        {
            TotalClientData = new Dictionary<string, ClientData>();
        }

        public static void AddClient(string username, ClientData clientData)
        {
            TotalClientData.Add(username, clientData);
        }
    }
}
