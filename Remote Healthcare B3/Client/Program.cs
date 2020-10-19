using Avans.TI.BLE;
using FietsSimulatorGUI;
using Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Client
{
    class Program
    {
        private static TcpClient Client;
        private static NetworkStream Stream;
        private static byte[] Buffer = new byte[1024];
        private static byte[] TotalBuffer = new byte[0];
        private static string Username;

        private static bool LoggedIn = false;
        private static bool RunningTraining = false;
        private static bool UseRealBike = false;
        private static BikeData Data;
        private static VRController VrController;
        private static List<string> Messages = new List<string>();

        private static BLE BleBike;
        private static BLE BleHeart;

        private static float LastSpeed;
        private static float LastHeartRate;
        private static float LastResistance;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome user!");
            Console.WriteLine("Whats your name? ");
            Username = Console.ReadLine();
            //username = "jkb";

            VrController = new VRController();
            IBike bike;
            if (UseRealBike)
            {
                InitBLEConnection();
                bike = new RealBike(BleBike,BleHeart);
                LastResistance = 0;
                LastSpeed = -1;
                LastHeartRate = -1;
            }
            else
            {
                Data = new BikeData(5, 120, 30, 5);
                LastSpeed = 5;
                LastHeartRate = 120;
                LastResistance = 30;
                bike = new SimBike(Data);
            }
            bike.OnSpeed += Bike_OnSpeed;
            bike.OnHeartRate += Bike_OnHeartrate;
            bike.OnSend += Bike_OnSend;

            Client = new TcpClient();
            Client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            while (true)
            {
                if (RunningTraining)
                {
     
                    if(!UseRealBike)
                    {
                        if (Console.ReadLine() == "")
                        {

                            Console.WriteLine("Input Command(Speed/HeartRate): ");
                            string command = Console.ReadLine();
                            switch (command)
                            {
                                case "Speed":
                                    Console.WriteLine("Input Speed: ");
                                    float speed = float.Parse(Console.ReadLine());
                                    Console.WriteLine(speed.ToString());
                                    Data.Speed = speed;
                                    break;
                                case "HeartRate":
                                    Console.WriteLine("Input HeartRAte: ");
                                    int heartRate = int.Parse(Console.ReadLine());
                                    Data.HeartRate = heartRate;
                                    break;
                                default:
                                    Console.WriteLine($"{command} is not a valid input!");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void Bike_OnSend(object sender, float e)
        {
            if (RunningTraining)
            {
                SendData(LastSpeed, LastHeartRate, LastResistance);
            }
        }

        private static void Bike_OnSpeed(object sender, float e)
        {
            if (RunningTraining)
            {
                //Console.WriteLine($"Speed: {e}");
                LastSpeed = e;
            }
        }

        private static void Bike_OnHeartrate(object sender, float e)
        {
            if (RunningTraining)
            {
                //Console.WriteLine($"HeartRate: {e}");
                LastHeartRate = e;
            }
        }

    

        private static void OnConnect(IAsyncResult ar)
        {
            Client.EndConnect(ar);
            Stream = Client.GetStream();
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
            Write($"login\r\n{Username}");

        }

        private static void OnRead(IAsyncResult ar)
        {
            int receivedBytes = Stream.EndRead(ar);
            TotalBuffer = Concat(TotalBuffer, Buffer, receivedBytes);

            while (TotalBuffer.Length > 8)
            {
                int encryptedLength = BitConverter.ToInt32(TotalBuffer, 0);
                int decryptedLength = BitConverter.ToInt32(TotalBuffer, 4);

                if (TotalBuffer.Length >= 8 + encryptedLength)
                {
                    byte[] PartialBuffer = TotalBuffer.Skip(8).Take(encryptedLength).ToArray();
                    string Decrypted = Crypting.DecryptStringFromBytes(PartialBuffer);

                    string[] packetData = Regex.Split(Decrypted, "\r\n");
                    HandleData(packetData);
                    TotalBuffer = TotalBuffer.Skip(encryptedLength + 8).Take(TotalBuffer.Length - encryptedLength - 8).ToArray();
                }
                else
                {
                    break;
                }
            }
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        }

        private static byte[] Concat(byte[] b1, byte[] b2, int b2count)
        {
            byte[] total = new byte[b1.Length + b2count];
            System.Buffer.BlockCopy(b1, 0, total, 0, b1.Length);
            System.Buffer.BlockCopy(b2, 0, total, b1.Length, b2count);
            return total;
        }

        private static void Write(string data)
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

        private static void HandleData(string[] packetData)
        {
            //Console.WriteLine($"Packet ontvangen: {packetData[0]}");

            switch (packetData[0])
            {
                case "login":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Connected");
                        LoggedIn = true;
                    }
                    else if (packetData[1] == "error")
                    {
                        Console.WriteLine(packetData[2]);
                        Console.WriteLine("Whats your name? ");
                        Username = Console.ReadLine();
                        Write($"login\r\n{Username}");
                    }
                    else
                        Console.WriteLine(packetData[1]);
                    break;

                case "data":
                    //Console.WriteLine(packetData[1]);
                    break;

                case "chatToAll":
                    if (packetData[1].Equals("message"))
                    {
                        string message = packetData[2];
                        Messages.Add(message);
                        Console.WriteLine(message);
                        VrController.UpdateChatPanel(Messages.ToArray());
                    }
                    break;

                case "directMessage":
                    if (packetData[1].Equals("message"))
                    {
                        string message = packetData[2];
                        Messages.Add(message);
                        Console.WriteLine(message);
                        VrController.UpdateChatPanel(Messages.ToArray());
                    }
                    break;

                case "StartTraining":
                    RunningTraining = true;
                    Console.WriteLine("Started Training");
                    break;

                case "StopTraining":
                    RunningTraining = false;
                    Console.WriteLine("Stopped Training");
                    VrController.UpdateBikePanel(0, 0, 0);
                    break;

                case "SetResistance":
                    SendResistance(int.Parse(packetData[1]));
                    break;

                default:
                    Console.WriteLine("Did not understand: "+ packetData[0]);
                    break;
            }

        }

        public static async void SendResistance(int resistance)
        {
            Console.WriteLine($"Send resistance {resistance}");
            Console.WriteLine($"lastResistance {LastResistance}");
            LastResistance = resistance;
            if (UseRealBike)
            {
                //Console.WriteLine($"Resistance: {lastResistance}");

                //Sending resistance to the Bluetooth device
                byte[] data = new byte[13];
                data[0] = 0x4A; // Sync byte
                data[1] = 0x09; // Length byte
                data[2] = 0x4E; // Type byte
                data[3] = 0x05; // Channel byte 
                data[4] = 0x30; // Data page nr
                data[5] = 0xff; // Not in use
                data[6] = 0xff; // Not in use
                data[7] = 0xff; // Not in use
                data[8] = 0xff; // Not in use
                data[9] = 0xff; // Not in use
                data[10] = 0xff; // Not in use
                data[11] = (byte)(LastResistance * 2); // Resistance percentage /2
                data[12] = 0; // Checksum

                byte previous = (byte)(data[0] ^ data[1]);

                for (int i = 2; i < data.Length - 1; i++)
                {
                    previous = (byte)(previous ^ data[i]);
                }

                //Console.WriteLine($"\n\n{previous}\n\n");
                data[12] = previous;

                await BleBike.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", data);
            }
        }

        public async static void InitBLEConnection()
        {
            BleBike = new BLE();
            BleHeart = new BLE();
            bool AlreadySubscribed = false;
            if (!AlreadySubscribed)
            {

                AlreadySubscribed = true;

                int errorCode = 0;

                Thread.Sleep(1000); // We need some time to list available devices

                // List available devices
                List<String> bleBikeList = BleBike.ListDevices();
                Console.WriteLine("Devices found: ");
                foreach (var name in bleBikeList)
                {
                    Console.WriteLine($"Device: {name}");
                }

                // Connecting
    
                // __TODO__ Error check

                var services = BleBike.GetServices;
                foreach (var service in services)
                {
                    Console.WriteLine($"Service: {service}");

                }

            }
        }

        public static void SendData(float speed, float heartRate, float resistance)
        {

            if (LoggedIn)
            {
                VrController.UpdateBikePanel(speed, heartRate, resistance);   
                DateTime now = DateTime.Now;
                int hour = now.Hour;
                int minute = now.Minute;
                int second = now.Second;
                int day = now.Day;
                int month = now.Month;
                int year = now.Year;
                int totalSeconds = hour * 60 * 60 + minute * 60 + second; 

                string message = "data\r\n" +
                    $"{speed}\r\n" +
                    $"{heartRate}\r\n" +
                    $"{resistance}\r\n" +
                    $"{totalSeconds}\r\n" +
                    $"{day}\r\n" +
                    $"{month}\r\n" +
                    $"{year}";
                Write(message);
            }
        }
    }
}
