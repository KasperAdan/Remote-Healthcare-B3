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
        public ClientData clientData;
        #endregion

        public string UserName { get; set; }
        public bool IsDoctor { get; set; }
        public bool isOnline { get; set; }


        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.clientData = new ClientData();

            this.IsDoctor = false;
            this.isOnline = true;
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
            //Console.WriteLine($"Got a packet: {packetData[0]}");
            switch (packetData[0])
            {
                case "login":
                    if (!assertPacketData(packetData, 2))
                        return;
                    this.UserName = packetData[1];
                    Console.WriteLine($"User {this.UserName} is connected");

                    Write("login\r\nok");
                    if (AllClients.totalClients.ContainsKey(UserName))
                    {
                        Client clientData;
                        AllClients.totalClients.TryGetValue(this.UserName, out clientData);
                        this.clientData = clientData.clientData;
                        //AllClients.Remove(this.UserName);
                       
                    }
                    else
                    {
                        Write($"AddClient\r\n{UserName}");
                    }
                    AllClients.Add(UserName, this);

                    

                    break;

                case "data":
                    if (!assertPacketData(packetData, 5))
                        return;
                    this.clientData.AddData(packetData[1], packetData[2], packetData[3], packetData[4]);
                    Write("data\r\nData Recieved");
                    this.clientData.PrintData();

                    //send real time data to all connected doctors
                    foreach (Client client in AllClients.totalClients.Values)
                    {
                        if (client.IsDoctor && client.isOnline)
                        {
                            client.Write($"RealTimeData\r\n{packetData[1]}\r\n{packetData[2]}\r\n{packetData[3]}\r\n{packetData[4]}");
                        }
                    }
                    break;

                case "DoctorLogin":
                    Console.WriteLine("DoctorLogin received");
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
                            this.UserName = username;
                            Console.WriteLine("correct password");
                            this.IsDoctor = true;
                            Write("DoctorLogin\r\nok");
                            AllClients.Add(username, this);
                        }
                        else
                        {
                            Console.WriteLine("incorrect password");
                            Write("DoctorLogin\r\nerror\r\nIncorrect password");
                        }
                    }
                    else
                    {
                        Console.WriteLine("incorrect username");
                        Write("DoctorLogin\r\nerror\r\nIncorrect username");
                    }
                    break;

                case "GetHistoricData": //doctor wants historic data
                    if (!assertPacketData(packetData, 2) || !this.IsDoctor)
                        return;
                    string dataUsername = packetData[1];
                    Client userClient;

                    bool gotValue = AllClients.totalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("GetHistoricData\r\nerror\r\nUsername not found");
                    }
                    else
                    {
                        Write($"SendHistoricData\r\n{userClient.clientData.GetJson().ToString()}");
                    }
                    break;

                case "GetRealtimeData":
                    if (!IsDoctor)
                        return;
                    //volgens mij zijn we deze vergeten en moeten we deze nog doen!!!
                    break;
                case "StartTraining":
                    //Server ontvangt dit en moet dit doorsturen naar bijbehorende Client
                    if (!assertPacketData(packetData, 2) || !this.IsDoctor)
                        return;

                    dataUsername = packetData[1];
                    gotValue = AllClients.totalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("StartTraining\r\nerror\r\nUsername not found");
                    }
                    else
                    {
                        if (!userClient.isOnline)
                        {
                            Write("StartTraining\r\nerror\r\nUser not online");
                        }
                        else
                        {
                            userClient.Write("StartTraining");
                            Write("StartTraining\r\nok");
                        }
                    }
                    break;

                case "StopTraining":
                    //Server ontvangt dit en moet dit doorsturen naar bijbehorende Client
                    if (!assertPacketData(packetData, 2) || !this.IsDoctor)
                        return;

                    dataUsername = packetData[1];
                    gotValue = AllClients.totalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("StopTraining\r\nerror\r\nUsername not found");
                    }
                    else
                    {
                        if (!userClient.isOnline)
                        {
                            Write("StopTraining\r\nerror\r\nUser not online");
                        }
                        else
                        {
                            userClient.Write("StopTraining");
                            Write("StopTraining\r\nok");
                        }
                    }
                    break;

                case "chatToAll":
                    if (!assertPacketData(packetData, 2))
                        return;
                    if (this.IsDoctor) { 
                        string messageToAll = packetData[1];
                        foreach (Client client in AllClients.totalClients.Values)
                        {
                            if (client.isOnline)
                            {
                                client.Write($"chatToAll\r\nmessage\r\n[{this.UserName}]: {messageToAll}");
                            }
                        }
                        Write($"chatToAll\r\nok");
                     }
                    else
                    {
                        Write($"chatToAll\r\nerror\r\nOnly doctors can chat to all");
                    }
                    break;
                case "directMessage":
                    if (!assertPacketData(packetData, 3))
                        return;
                    string messageTo = packetData[1];
                    string message = packetData[2];
                    Client messageToClient;
                    gotValue = AllClients.totalClients.TryGetValue(messageTo, out messageToClient);
                    if (gotValue)
                    {
                        if (this.IsDoctor || messageToClient.IsDoctor) {
                            if (messageToClient.isOnline)
                            {
                                messageToClient.Write($"directMessage\r\nmessage\r\n({this.UserName}): {message}");
                                Write($"directMessage\r\nok");
                            }
                            else
                            {
                                Write($"directMessage\r\nerror\r\nTarget client is not online");
                            }
                        }
                    }
                    else
                    {
                        Write($"directMessage\r\nerror\r\nNeither client is a doctor");
                    }
                    break;
                case "GetClients":
                    if (!IsDoctor && !assertPacketData(packetData, 1))
                    {
                        return;
                    }
                    string allUsernames = "";
                    int userAmount = 0;
                    foreach(Client client in AllClients.totalClients.Values)
                    {
                        if (!client.IsDoctor)
                        {
                            allUsernames += "\r\n" + client.UserName;
                            userAmount++;
                        }
                    }
                    message = $"GetClients\r\nok\r\n{userAmount}{allUsernames}";
                    Write(message);
                    break;
                default:
                    Console.WriteLine("Did not understand: " + packetData[0]);
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

        public void Disconnect()
        {
            this.isOnline = false;
        }
    }
}