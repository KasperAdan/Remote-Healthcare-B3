using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DokterApplicatie
{
    static class Program
    {

        private static string password;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;

        private static bool loggedIn = false;
        private static bool useRealBike = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

       
        static void Main()
        {

            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            Write($"login\r\n{username}");

        }

        private static void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);
            string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            totalBuffer += receivedText;

            while (totalBuffer.Contains("\r\n\r\n"))
            {
                string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
                totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
                string[] packetData = Regex.Split(packet, "\r\n");
                handleData(packetData);
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        private static void Write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
    

        private static void handleData(string[] packetData)
        {
            //Console.WriteLine($"Packet ontvangen: {packetData[0]}");

            switch (packetData[0])
            {
                case "login":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Connected");
                        loggedIn = true;
                    }
                    else
                        Console.WriteLine(packetData[1]);
                    break;
                case "data":
                    //Console.WriteLine(packetData[1]);
                    break;
            }

        }

    }
}
