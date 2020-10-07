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
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        public ClientData clientData;
        #endregion

        public string UserName { get; set; }
        public bool IsDoctor { get; set; }
        public bool isOnline { get; set; }

        public CryptoStream crStreamRead;

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.clientData = new ClientData();

            this.IsDoctor = false;
            this.isOnline = true;
            this.stream = this.tcpClient.GetStream();
            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            cryptic.Key = ASCIIEncoding.ASCII.GetBytes("ABCDEFGH");
            cryptic.IV = ASCIIEncoding.ASCII.GetBytes("ABCDEFGH");

            crStreamRead = new CryptoStream(stream, cryptic.CreateDecryptor(), CryptoStreamMode.Read);
            crStreamRead.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        #region connection stuff
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = stream.EndRead(ar);

                string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);


                var Key = new byte[32]
                    { 9, 9 , 9, 9, 9, 9 , 9, 9, 9, 9 , 9, 9, 9, 9 , 9, 9,  9, 9 , 9, 9, 9, 9 , 9, 9, 9, 9 , 9, 9, 9, 9 , 9, 9};
                var IV = new byte[16] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };

                for (int i = 0; i < receivedBytes; i++)
                {
                    Console.WriteLine(receivedText[i]);
                }

                byte[] PartialBuffer = buffer.Take(receivedBytes).ToArray();

               
               String Decrypted = DecryptStringFromBytes(PartialBuffer, Key, IV);
               ;

               totalBuffer += Decrypted;
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
            crStreamRead.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
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

                    if (AllClients.totalClients.ContainsKey(UserName))
                    {
                        Client clientData;
                        AllClients.totalClients.TryGetValue(this.UserName, out clientData);
                        this.clientData = clientData.clientData;
                        //AllClients.Remove(this.UserName);
                    }
                    AllClients.Add(UserName, this);

                    Write("login\r\nok");
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

                case "DocterLogin":
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
                            Write("DocterLogin\r\nok");
                            AllClients.Add(username, this);
                        }
                        else
                        {
                            Write("DocterLogin\r\nerror\r\nIncorrect password");
                        }
                    }
                    else
                    {
                        Write("DocterLogin\r\nerror\r\nIncorrect username");
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
                                Write($"chatToAll\r\nmessage\r\n[{this.UserName}]: {messageToAll}");
                            }
                        }
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
                                messageToClient.Write($"directMessage\r\nmessage\r\n({this.UserName}: {message})");
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
            var Key = new byte[32]
                {9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9};
            var IV = new byte[16] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };


            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");

            var dataStringEncrypted = EncryptStringToBytes(data + "\r\n\r\n", Key, IV);


            Debug.WriteLine("Non encrypted.. " + Encoding.ASCII.GetString(dataAsBytes));

            Debug.WriteLine("Encrypted " + Encoding.ASCII.GetString(dataStringEncrypted));

            stream.Write(dataStringEncrypted, 0, dataStringEncrypted.Length);

            stream.Flush();
        }

        public void Disconnect()
        {
            this.isOnline = false;
        }


        //Encrypt and decrypt methods
        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }


    }


}