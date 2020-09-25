﻿using Avans.TI.BLE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace Client
{
    class Program
    {
        private static string password;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;

        private static bool loggedIn = false;
        private static bool useRealBike = true;

        private static BLE bleBike;
        private static BLE bleHeart;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome user!");
            Console.WriteLine("Whats your name? ");
            username = Console.ReadLine();

            IBike bike;
            if (useRealBike)
            {
                initBLEConnection();
                bike = new RealBike(bleBike,bleHeart);
            }
            else
            {
                bike = new SimBike();
            }
            bike.OnSpeed += Bike_OnSpeed;
            bike.OnHeartRate += Bike_OnHeartrate;
            Console.ReadLine();

            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            //VRController vrController = new VRController(); // WHY U NO WORK
            bool sendData = false;
            while (true)
            {
                if (useRealBike)
                {
                    if (Console.ReadLine() == "")
                    {
                        Console.WriteLine("Input resistance: ");
                        int resistance = int.Parse(Console.ReadLine());
                        sendResistance(resistance);
                    }

                }
                else
                {

                }
            }
        }

        private static void Bike_OnSpeed(object sender, float e)
        {
            //Console.WriteLine("Speed: " + e);
        }

        private static void Bike_OnHeartrate(object sender, float e)
        {
            //Console.WriteLine("Heartrate: " + e);
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
            Console.WriteLine($"Packet ontvangen: {packetData[0]}");

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
                    Console.WriteLine(packetData[1]);
                    break;
            }

        }

        public static async void sendResistance(int resistance)
        {
            if (useRealBike)
            {
                Console.WriteLine($"Resistance: {resistance}");

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
                data[11] = (byte)(resistance * 2); // Resistance percentage /2
                data[12] = 0; // Checksum

                byte previous = (byte)(data[0] ^ data[1]);

                for (int i = 2; i < data.Length - 1; i++)
                {
                    previous = (byte)(previous ^ data[i]);
                }

                Console.WriteLine($"\n\n{previous}\n\n");
                data[12] = previous;

                for (int i = 0; i < data.Length; i++)
                {
                    Console.WriteLine(data[i]);
                }
                await bleBike.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", data);
            }
        }

        public async static void initBLEConnection()
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
    }
}
