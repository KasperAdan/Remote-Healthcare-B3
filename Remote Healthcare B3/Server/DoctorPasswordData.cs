using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Server
{
     static class DoctorPasswordData
    {
        public static Dictionary<string, string> DoctorPassWords { get; set; }

        public static void Init()
        {
            DoctorPassWords = new Dictionary<string, string>();
            //add passwords
            DoctorPassWords.Add("Lars", "1234");
            DoctorPassWords.Add("Test", "1234");
        }

        public static void Add(string username, string password)
        {
            DoctorPassWords.Add(username, password);
        }

    }
}
