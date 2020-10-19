using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Server
{
    internal class Client
    {
        #region connection stuff
        private TcpClient TCPClient;
        private NetworkStream Stream;
        private byte[] Buffer = new byte[1024];
        private byte[] TotalBuffer = new byte[0];
        public ClientData ClientData;
        #endregion

        public string UserName { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsOnline { get; set; }

        public Client(TcpClient tcpClient)
        {
            this.TCPClient = tcpClient;
            this.ClientData = new ClientData();

            this.IsDoctor = false;
            this.IsOnline = true;
            this.Stream = this.TCPClient.GetStream();
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        }
        #region connection stuff
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = Stream.EndRead(ar);
                TotalBuffer = Concat(TotalBuffer, Buffer, receivedBytes);


                while(TotalBuffer.Length > 8)
                {
                    int encryptedLength = BitConverter.ToInt32(TotalBuffer, 0);
                    int decryptedLength = BitConverter.ToInt32(TotalBuffer, 4);

                    if(TotalBuffer.Length >= 8 + encryptedLength)
                    {

                        //string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                        byte[] PartialBuffer = TotalBuffer.Skip(8).Take(encryptedLength).ToArray();
                        String Decrypted = Crypting.DecryptStringFromBytes(PartialBuffer);
                        
                        
                        string[] packetData = Regex.Split(Decrypted, "\r\n");
                        HandleData(packetData);
                        TotalBuffer = TotalBuffer.Skip(encryptedLength + 8).Take(TotalBuffer.Length - encryptedLength - 8).ToArray();
                    } else
                    {
                        break;
                    }
                }
            }
            catch (IOException)
            {
                Program.Disconnect(this);
                return;
            }

            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        }

        private byte[] Concat(byte[] b1, byte[] b2, int b2count)
        {
            byte[] total = new byte[b1.Length + b2count];
            System.Buffer.BlockCopy(b1, 0, total, 0, b1.Length);
            System.Buffer.BlockCopy(b2, 0, total, b1.Length, b2count);
            return total;
        }
        #endregion

        private void HandleData(string[] packetData)
        {
            //Console.WriteLine($"Got a packet: {packetData[0]}");
            switch (packetData[0])
            {
                case "login":
                    if (!AssertPacketData(packetData, 2))
                        return;
                    this.UserName = packetData[1];
                    Console.WriteLine($"User {this.UserName} is connected");

                    if (DoctorPasswordData.DoctorPassWords.ContainsKey(this.UserName))
                    {
                        Console.WriteLine("Username is in use by doctor");
                        Write("login\r\nerror\r\nThe username is in use");
                        return;
                    }


                    if (AllClients.TotalClients.ContainsKey(UserName))
                    {
                        Client client;
                        AllClients.TotalClients.TryGetValue(UserName,out client);

                        if (client.IsOnline)
                        {
                            Write("login\r\nerror\r\nA user with the same name is already online");
                        }
                        else {
                            Client clientData;
                            AllClients.TotalClients.TryGetValue(this.UserName, out clientData);
                            this.ClientData = clientData.ClientData;
                            AllClients.Remove(this.UserName);
                            Write("login\r\nok");
                            AllClients.Add(UserName, this);

                        }
                       
                    }
                    else
                    {
                        foreach(Client client in AllClients.TotalClients.Values)
                        {
                            if (client.IsDoctor && client.IsOnline)
                            {
                                client.Write($"AddClient\r\n{UserName}");
                            }
                        }
                        Write("login\r\nok");
                        AllClients.Add(UserName, this);
                    }
                    break;

                case "data":
                    if (!AssertPacketData(packetData, 8))
                        return;
                    this.ClientData.AddData(packetData[1], packetData[2], packetData[3], packetData[4], packetData[5], packetData[6], packetData[7]);
                    Write("data\r\nData Recieved");
                    this.ClientData.PrintData();

                    //send real time data to all connected doctors
                    foreach (Client client in AllClients.TotalClients.Values)
                    {
                        if (client.IsDoctor && client.IsOnline)
                        {
                            Console.WriteLine("Data size: "+this.ClientData.Data.Count);
                            string recentDataJson = JsonConvert.SerializeObject(this.ClientData.Data);
                            Console.WriteLine("Sending realtime: "+ recentDataJson);
                            client.Write($"RealTimeData\r\n{this.UserName}\r\n{recentDataJson}");
                        }
                    }
                    break;

                case "DoctorLogin":
                    Console.WriteLine("DoctorLogin received");
                    if (!AssertPacketData(packetData, 3))
                        return;
                    string username = packetData[1];
                    string password = packetData[2];
                    //check password
                    Dictionary<string, string> passwords = DoctorPasswordData.DoctorPassWords;
                    string dictionaryPassword;
                    if (AllClients.TotalClients.ContainsKey(username))
                    {
                        Client client;
                        AllClients.TotalClients.TryGetValue(username, out client);
                        if (client.IsOnline)
                        {
                            Write("DoctorLogin\r\nerror\r\nYou are already logged in somewhere else!");
                            return;
                        }
                    }
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
                    if (!AssertPacketData(packetData, 2) || !this.IsDoctor)
                        return;
                    string dataUsername = packetData[1];
                    Client userClient;

                    bool gotValue = AllClients.TotalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("GetHistoricData\r\nerror\r\nUsername not found");
                    }
                    else
                    {

                        //We moeten de graph lijst pakken i.p.v. de getJson!!!
                        string historicDataJson = JsonConvert.SerializeObject(userClient.ClientData.Graphs);
                        Write($"GetHistoricData\r\n{dataUsername}\r\n{historicDataJson}");
                    }
                    break;

                case "GetRealtimeData":
                    if (!IsDoctor)
                        return;
                    
                    break;
                case "StartTraining":
                    //Server ontvangt dit en moet dit doorsturen naar bijbehorende Client
                    if (!AssertPacketData(packetData, 2) || !this.IsDoctor)
                        return;

                    dataUsername = packetData[1];
                    gotValue = AllClients.TotalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("StartTraining\r\nerror\r\nUsername not found");
                    }
                    else
                    {
                        if (!userClient.IsOnline)
                        {
                            Write("StartTraining\r\nerror\r\nUser not online");
                        }
                        else
                        {
                            userClient.Write("StartTraining");
                            Write("StartTraining\r\nok");
                            userClient.ClientData.StartGraph();
                        }
                    }
                    break;

                case "StopTraining":
                    //Server ontvangt dit en moet dit doorsturen naar bijbehorende Client
                    if (!AssertPacketData(packetData, 2) || !this.IsDoctor)
                        return;

                    dataUsername = packetData[1];
                    gotValue = AllClients.TotalClients.TryGetValue(dataUsername, out userClient);

                    if (!gotValue)
                    {
                        Write("StopTraining\r\nerror\r\nUsername not found");
                    }
                    else
                    {
                        if (!userClient.IsOnline)
                        {
                            Write("StopTraining\r\nerror\r\nUser not online");
                        }
                        else
                        {
                            userClient.Write("StopTraining");
                            Write("StopTraining\r\nok");
                            userClient.ClientData.FinishGraph();
                        }
                    }
                    break;

                case "chatToAll":
                    if (!AssertPacketData(packetData, 2))
                        return;
                    if (this.IsDoctor) { 
                        string messageToAll = packetData[1];
                        foreach (Client client in AllClients.TotalClients.Values)
                        {
                            if (client.IsOnline)
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
                    if (!AssertPacketData(packetData, 3))
                        return;
                    string messageTo = packetData[1];
                    string message = packetData[2];
                    Client messageToClient;
                    gotValue = AllClients.TotalClients.TryGetValue(messageTo, out messageToClient);
                    if (gotValue)
                    {
                        if (this.IsDoctor || messageToClient.IsDoctor) {
                            if (messageToClient.IsOnline)
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
                    if (!IsDoctor || !AssertPacketData(packetData, 1))
                    {
                        return;
                    }
                    string allUsernames = "";
                    int userAmount = 0;
                    foreach(Client client in AllClients.TotalClients.Values)
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
                case "SetResistance":
                    if (!AssertPacketData(packetData, 3)) { return; }
                    Client clientResistance;
                    AllClients.TotalClients.TryGetValue(packetData[1], out clientResistance);
                    if(clientResistance == null)
                    {
                        Write("SetResistance\r\nerror\r\nDid not find user");
                    }
                    else
                    {
                        clientResistance.Write($"SetResistance\r\n{packetData[2]}");
                    }
                    
                    break;
                default:
                    Console.WriteLine("Did not understand: " + packetData[0]);
                    break;

            }
        }

        private bool AssertPacketData(string[] packetData, int requiredLength)
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
            var dataAsBytes = Encoding.ASCII.GetBytes(data + "\r\n\r\n");

            var dataStringEncrypted = Crypting.EncryptStringToBytes(data + "\r\n\r\n");


            Debug.WriteLine("Non encrypted.. " + Encoding.ASCII.GetString(dataAsBytes));

            Debug.WriteLine("Encrypted " + Encoding.ASCII.GetString(dataStringEncrypted));

            Stream.Write(BitConverter.GetBytes(dataStringEncrypted.Length), 0, 4);
            Stream.Write(BitConverter.GetBytes(dataAsBytes.Length), 0, 4);

            Stream.Write(dataStringEncrypted, 0, dataStringEncrypted.Length);

            Stream.Flush();
        }

        public void Disconnect()
        {
            this.IsOnline = false;
        }
    }
}