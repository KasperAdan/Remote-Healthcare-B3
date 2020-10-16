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
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;

        private static bool loggedIn = false;
        private static bool runningTraining = false;
        private static bool useRealBike = false;
        private static BikeData data;
        private static VRController vrController;

        private static BLE bleBike;
        private static BLE bleHeart;

        private static float lastSpeed;
        private static float lastHeartRate;
        private static float lastResistance;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome user!");
            Console.WriteLine("Whats your name? ");
            username = Console.ReadLine();
            //username = "jkb";

            vrController = new VRController();
            IBike bike;
            if (useRealBike)
            {
                InitBLEConnection();
                bike = new RealBike(bleBike,bleHeart);
                lastResistance = 0;
                lastSpeed = -1;
                lastHeartRate = -1;
            }
            else
            {
                data = new BikeData(5, 120, 30, 5);
                lastSpeed = 5;
                lastHeartRate = 120;
                lastResistance = 30;
                bike = new SimBike(data);
            }
            bike.OnSpeed += Bike_OnSpeed;
            bike.OnHeartRate += Bike_OnHeartrate;
            bike.OnSend += Bike_OnSend;

            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            while (true)
            {
                if (runningTraining)
                {
                    if (useRealBike)
                    {
                        if (Console.ReadLine() == "")
                        {
                            Console.WriteLine("Input resistance: (First type a 0, after that the resistance)");

                            int resistance = int.Parse(Console.ReadLine());
                            lastResistance = resistance;
                            SendResistance((int)lastResistance);
                        }

                    }
                    else
                    {
                        if (Console.ReadLine() == "")
                        {

                            Console.WriteLine("Input Command(Speed/HeartRate/Resistance): ");
                            string command = Console.ReadLine();
                            switch (command)
                            {
                                case "Speed":
                                    Console.WriteLine("Input Speed: ");
                                    float speed = float.Parse(Console.ReadLine());
                                    Console.WriteLine(speed.ToString());
                                    data.Speed = speed;
                                    break;
                                case "HeartRate":
                                    Console.WriteLine("Input HeartRAte: ");
                                    int heartRate = int.Parse(Console.ReadLine());
                                    data.HeartRate = heartRate;
                                    break;
                                case "Resistance":
                                    Console.WriteLine("Input Resistance: ");
                                    float resistance = float.Parse(Console.ReadLine());
                                    lastResistance = resistance;
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
            if (runningTraining)
            {
                SendData(lastSpeed, lastHeartRate, lastResistance);
            }
        }

        private static void Bike_OnSpeed(object sender, float e)
        {
            if (runningTraining)
            {
                //Console.WriteLine($"Speed: {e}");
                lastSpeed = e;
            }
        }

        private static void Bike_OnHeartrate(object sender, float e)
        {
            if (runningTraining)
            {
                //Console.WriteLine($"HeartRate: {e}");
                lastHeartRate = e;
            }
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

            string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

            byte[] PartialBuffer = buffer.Take(receivedBytes).ToArray();


            String Decrypted = Crypting.DecryptStringFromBytes(PartialBuffer);
            ;

            totalBuffer += Decrypted;

            while (totalBuffer.Contains("\r\n\r\n"))
            {
                string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
                totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
                string[] packetData = Regex.Split(packet, "\r\n");
                HandleData(packetData);
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        private static void Write(string data)
        {
            var dataAsBytes = Encoding.ASCII.GetBytes(data + "\r\n\r\n");

            var dataStringEncrypted = Crypting.EncryptStringToBytes(data + "\r\n\r\n");


            Debug.WriteLine("Non encrypted.. " + Encoding.ASCII.GetString(dataAsBytes));

            Debug.WriteLine("Encrypted " + Encoding.ASCII.GetString(dataStringEncrypted));

            stream.Write(dataStringEncrypted, 0, dataStringEncrypted.Length);

            stream.Flush();
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
                        loggedIn = true;
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
                        Console.WriteLine(message);
                    }
                    break;

                case "directMessage":
                    if (packetData[1].Equals("message"))
                    {
                        string message = packetData[2];
                        Console.WriteLine(message);
                    }
                    break;

                case "StartTraining":
                    runningTraining = true;
                    Console.WriteLine("Started Training");
                    break;

                case "StopTraining":
                    runningTraining = false;
                    Console.WriteLine("Stopped Training");
                    break;

                default:
                    Console.WriteLine("Did not understand: "+ packetData[0]);
                    break;
            }

        }

        public static async void SendResistance(int resistance)
        {
            Console.WriteLine($"Send resistance {resistance}");
            Console.WriteLine($"lastResistance {lastResistance}");
            lastResistance = resistance;
            if (useRealBike)
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
                data[11] = (byte)(lastResistance * 2); // Resistance percentage /2
                data[12] = 0; // Checksum

                byte previous = (byte)(data[0] ^ data[1]);

                for (int i = 2; i < data.Length - 1; i++)
                {
                    previous = (byte)(previous ^ data[i]);
                }

                //Console.WriteLine($"\n\n{previous}\n\n");
                data[12] = previous;

                await bleBike.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", data);
            }
        }

        public async static void InitBLEConnection()
        {
            bleBike = new BLE();
            bleHeart = new BLE();
            bool AlreadySubscribed = false;
            if (!AlreadySubscribed)
            {

                AlreadySubscribed = true;

                int errorCode = 0;

                Thread.Sleep(1000); // We need some time to list available devices

                // List available devices
                List<String> bleBikeList = bleBike.ListDevices();
                Console.WriteLine("Devices found: ");
                foreach (var name in bleBikeList)
                {
                    Console.WriteLine($"Device: {name}");
                }

                // Connecting
    
                // __TODO__ Error check

                var services = bleBike.GetServices;
                foreach (var service in services)
                {
                    Console.WriteLine($"Service: {service}");

                }

            }
        }

        public static void SendData(float speed, float heartRate, float resistance)
        {

            if (loggedIn)
            {
                vrController.UpdateBikePanel(speed, heartRate, resistance);   
                DateTime now = DateTime.Now;
                int hour = now.Hour;
                int minute = now.Minute;
                int second = now.Second;
                int totalSeconds = hour * 60 * 60 + minute * 60 + second; 

                string message = "data\r\n" +
                    $"{speed}\r\n" +
                    $"{heartRate}\r\n" +
                    $"{resistance}\r\n" +
                    $"{totalSeconds}";
                Write(message);
            }
        }
    }
}
