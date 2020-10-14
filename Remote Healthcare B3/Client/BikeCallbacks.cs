using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Timers;
using Avans.TI.BLE;
using FietsSimulatorGUI;

namespace Client
{
    interface IBike
    {
        event EventHandler<float> OnSpeed;
        event EventHandler<float> OnHeartRate;
        event EventHandler<float> OnSend; 
    }



    public class SimBike : IBike
    {
        public event EventHandler<float> OnSpeed;
        public event EventHandler<float> OnHeartRate;
        public event EventHandler<float> OnSend;

        public BikeData data;

        public SimBike(BikeData data)
        {
            this.data = data;
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        private void WorkThreadFunction()
        {
            while (true)
            {
                OnSpeed?.Invoke(this, data.GetSpeed());
                OnHeartRate?.Invoke(this, data.GetHeartRate());
                OnSend?.Invoke(this, 0);
                Thread.Sleep(1000);
            }
        }
    }


    public class RealBike : IBike
    {
        public event EventHandler<float> OnSpeed;
        public event EventHandler<float> OnHeartRate;
        public event EventHandler<float> OnSend;

        public BLE bleBike;
        public BLE bleHeart;

        public System.Timers.Timer sendTimer;

        public RealBike(BLE bike, BLE heart)
        {
            this.bleBike = bike;
            this.bleHeart = heart;
            sendTimer = new System.Timers.Timer(1000);
            sendTimer.Elapsed += TimerCallBack;
            sendTimer.AutoReset = true;
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunctionRealBike));
            thread.Start();

        }


        async private void WorkThreadFunctionRealBike()
        {
 
            try
            {
                int errorCode;

                Console.WriteLine(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
                errorCode = await bleBike.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));

                // Set service
                errorCode = await bleBike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
                // __TODO__ error check

                sendTimer.Start();
                // Subscribe
                bleBike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
                errorCode = await bleBike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
                Console.WriteLine("subscribed to bike:");



                // Heart rate
                errorCode = await bleHeart.OpenDevice(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
                await bleHeart.SetService("HeartRate");

                bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
                await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");
                Console.WriteLine("subscribed to heart");

                Program.SendResistance(0);

                //Console.Read();

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
                    OnSpeed?.Invoke(this, Speed);
                        //Console.WriteLine("\n\tSpeed: " + Speed + "m/s");

                        //Console.WriteLine("\telapsed time: " + e.Data[6]/4.0 + " seconds");
                        //Console.WriteLine("\telapsed distance: " + e.Data[7] + " meters\n");

                    }
                }
                else if (e.ServiceName == "00002a37-0000-1000-8000-00805f9b34fb")
                {
                float HeartRate = e.Data[1];
                OnHeartRate?.Invoke(this, HeartRate);
                    //Console.WriteLine($"\n\tHeartRate: {HeartRate}bpm");
                }

                //Console.WriteLine("Received from {0}: {1}, {2}", e.ServiceName,
                //    BitConverter.ToString(e.Data).Replace("-", " "),
                //    Encoding.UTF8.GetString(e.Data));
            
        }


        private void TimerCallBack(object sender, ElapsedEventArgs e)
        {
            OnSend?.Invoke(this, 0);
        }



    }
}
