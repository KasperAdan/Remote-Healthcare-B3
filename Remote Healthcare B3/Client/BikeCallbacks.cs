using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Avans.TI.BLE;



namespace Client
{
    interface IBike
    {
        event EventHandler<float> OnSpeed;
        event EventHandler<float> OnHeartRate;
    }



    public class SimBike : IBike
    {
        public event EventHandler<float> OnSpeed;
        public event EventHandler<float> OnHeartRate;

        public SimBike()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        private void WorkThreadFunction()
        {
            while (true)
            {
                OnSpeed?.Invoke(this, 10.0f);
                OnHeartRate?.Invoke(this, 120.0f);
                Thread.Sleep(1000);
            }
        }
    }


    public class RealBike : IBike
    {
        public event EventHandler<float> OnSpeed;
        public event EventHandler<float> OnHeartRate;

        public RealBike(string bikeName)
        {
                System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunctionRealBike));
                thread.Start();
           
        }

        async private void WorkThreadFunctionRealBike()
        {

            try
            {
                BLE bleBike = new BLE();
                BLE bleHeart = new BLE();
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
                    Console.WriteLine(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
                    errorCode = errorCode = await bleBike.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
                    // __TODO__ Error check

                    var services = bleBike.GetServices;
                    foreach (var service in services)
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
                    errorCode = await bleHeart.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));


                    await bleHeart.SetService("HeartRate");

                    bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
                    await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");

                    Console.Read();

                }
            }
            catch (Exception ex)
            {
                // log errors
            }


        }

        private void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        { 
                //foreach (byte b in e.Data)
                //{
                //    Console.Write(b + " ");
                //}

                if (e.ServiceName == "6e40fec2-b5a3-f393-e0a9-e50e24dcca9e")
                {
                    if (e.Data[4] == 16)
                    {
                    float Speed = (e.Data[9] * 256 + e.Data[8]) / 1000.00f;
                    OnSpeed.Invoke(this, Speed);
                        Console.WriteLine("\n\tSpeed: " + Speed + "m/s");

                        //Console.WriteLine("\telapsed time: " + e.Data[6]/4.0 + " seconds");
                        //Console.WriteLine("\telapsed distance: " + e.Data[7] + " meters\n");

                    }
                }
                else if (e.ServiceName == "00002a37-0000-1000-8000-00805f9b34fb")
                {
                float HeartRate = e.Data[1];
                OnHeartRate?.Invoke(this, HeartRate);
                    Console.WriteLine($"\n\tHeartRate: {HeartRate}bpm");
                }

                //Console.WriteLine("Received from {0}: {1}, {2}", e.ServiceName,
                //    BitConverter.ToString(e.Data).Replace("-", " "),
                //    Encoding.UTF8.GetString(e.Data));
            
        }

        public async void sendResistance()
        {
            int resistance = (int.Parse(fysicalResistaceValue.Value.ToString()));
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
}
