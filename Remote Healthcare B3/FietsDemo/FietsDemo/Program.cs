using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int errorCode = 0;
            BLE bleBike = new BLE();
            BLE bleHeart = new BLE();
            Thread.Sleep(1000); // We need some time to list available devices

            // List available devices
            List<String> bleBikeList = bleBike.ListDevices();
            Console.WriteLine("Devices found: ");
            foreach (var name in bleBikeList)
            {
                Console.WriteLine($"Device: {name}");
            }

            // Connecting
            errorCode = errorCode = await bleBike.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
            // __TODO__ Error check

            var services = bleBike.GetServices;
            foreach(var service in services)
            {
                Console.WriteLine($"Service: {service}");
            }

            // Set service
            errorCode = await bleBike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
            // __TODO__ error check

            // Subscribe
            bleBike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            errorCode = await bleBike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

            // Heart rate
            errorCode =  await bleHeart.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));

            await bleHeart.SetService("HeartRate");

            bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");

            byte[] data = new byte[13];
            data[0] = 0xA4;
            data[1] = 0x09;
            data[2] = 0x4E;
            data[3] = 0x05;
            data[4] = 0x30;
            data[5] = 0xff;
            data[6] = 0xff;
            data[7] = 0xff;
            data[8] = 0xff;
            data[9] = 0xff;
            data[10] = 0xff;
            data[11] = 0x50;
            data[12] = 0;

            byte previous = (byte)(data[0] ^ data[1]);
            for (int i = 2; i < data.Length-1; i++)
            {
                previous = (byte)(previous ^ data[i]);
            }

            Console.WriteLine($"\n\n{previous}\n\n");
            data[12] = previous;

            await bleBike.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", data);

            Console.Read();
        }

        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {

            //foreach (byte b in e.Data)
            //{
            //    Console.Write(b + " ");
            //}
            if (e.ServiceName == "6e40fec2-b5a3-f393-e0a9-e50e24dcca9e")
            {
                if (e.Data[4] == 16)
                { 
                    Console.WriteLine("\n\tSpeed: " + (e.Data[9] * 256 + e.Data[8]) / 1000.00 + "m/s");
                    //Console.WriteLine("\telapsed time: " + e.Data[6]/4.0 + " seconds");
                    //Console.WriteLine("\telapsed distance: " + e.Data[7] + " meters\n");

                }
            }
            else if (e.ServiceName == "00002a37-0000-1000-8000-00805f9b34fb")
            {
                Console.WriteLine($"\n\tHeartRate: {e.Data[1]}bpm");
            }

            //Console.WriteLine("Received from {0}: {1}, {2}", e.ServiceName,
            //    BitConverter.ToString(e.Data).Replace("-", " "),
            //    Encoding.UTF8.GetString(e.Data));
        }

    }
}
