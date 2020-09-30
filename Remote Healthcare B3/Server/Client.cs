using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Server
{
    internal class Client
    {
        #region connection stuff
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        private ClientData clientData;
        #endregion

        public string UserName { get; set; }
        public bool IsDoctor { get; set; }


        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.clientData = new ClientData();

            this.IsDoctor = false;
            this.stream = this.tcpClient.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        #region connection stuff
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = stream.EndRead(ar);
                string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                totalBuffer += receivedText;
            }
            catch (IOException)
            {
                Program.Disconnect(this);
                return;
            }

            while (totalBuffer.Contains("\r\n\r\n"))
            {
                string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
                totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
                string[] packetData = Regex.Split(packet, "\r\n");
                handleData(packetData);
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        #endregion

        private void handleData(string[] packetData)
        {
            Console.WriteLine($"Got a packet: {packetData[0]}");
            switch (packetData[0])
            {
                case "login":
                    if (!assertPacketData(packetData, 2))
                        return;
                    this.UserName = packetData[1];
                    Console.WriteLine($"User {this.UserName} is connected");
                    Write("login\r\nok");
                    break;
                case "data":
                    if (!assertPacketData(packetData, 5))
                        return;
                    this.clientData.AddData(packetData[1], packetData[2], packetData[3], packetData[4]);
                    Write("data\r\nData Recieved");
                    this.clientData.PrintData();
                    break;
                case "DoctorLogin":
                    if (!assertPacketData(packetData, 3))
                        return;
                    string username = packetData[1];
                    string password = packetData[2];
                    //check password
                    Dictionary<string, string> passwords = DoctorPasswordData.DoctorPassWords;
                    string dictionaryPassword;
                    if (passwords.ContainsKey(username))
                    {
                        passwords.TryGetValue(username, out dictionaryPassword);
                        if (dictionaryPassword.Equals(password))
                        {
                            this.IsDoctor = true;
                            Write("DoctorLogin\r\nok");
                        }
                        else
                        {
                            Write("DoctorLogin\r\nerror\r\nIncorrect password");
                        }
                    }
                    else
                    {
                        Write("DoctorLogin\r\nerror\r\nIncorrect username");
                    }
                    break;
            }


        }

        private bool assertPacketData(string[] packetData, int requiredLength)
        {
            if (packetData.Length < requiredLength)
            {
                Write("error");
                return false;
            }
            return true;
        }

        public void Write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
    }
}