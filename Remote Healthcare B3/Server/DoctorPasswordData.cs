using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Server
{
     static class DoctorPasswordData
    {
        public static Dictionary<string, string> DoctorPassWords { get; set; }

        public static void init()
        {
            DoctorPassWords = new Dictionary<string, string>();
            //add passwords
        }

        public static void Add(string username, string password)
        {
            DoctorPassWords.Add(username, password);
        }

    }
}
