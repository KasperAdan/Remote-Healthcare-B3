using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Avans.TI.BLE;


namespace FietsSimulatorGUI
{
    public partial class BikeSimulator : Form
    {

        public int ValuePower { get; set; }
        public int ValueSpeed { get; set; }
        public int ValueHeartRate { get; set; }

        public Boolean IsVirtualRunning { get; set; }

        public Boolean AlreadySubscribed { get; set; }
        public static Boolean IsFysicalRunning { get; set; }

        public Random Random { get; set; }

        public BikeData BikeData { get; set; }

        BLE bleBike = new BLE();
        BLE bleHeart = new BLE();


        public BikeSimulator()
        {
            InitializeComponent();
            ValuePower = 10;
            ValueSpeed = 10;
            ValueHeartRate = 50;
            IsVirtualRunning = false;
            IsFysicalRunning = false;
            Random = new Random();
            BikeData = new BikeData(ValueSpeed, ValueHeartRate, ValuePower, 10);
            AlreadySubscribed = false;

        }

        private void StartVirtual_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start geklikt!");
            Console.WriteLine($"Waardes zijn: {ValuePower}, {ValueSpeed}, {ValueHeartRate}");


            ValuePower = (int)virtualResistanceValue.Value;
            ValueSpeed = (int)VirtualSpeedValue.Value;
            ValueHeartRate = (int)VirtualHeartRateValue.Value;

            BikeData.Resistance = ValuePower;
            BikeData.Speed = ValueSpeed;
            BikeData.HeartRate = ValueHeartRate;

            IsVirtualRunning = true;
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        public void WorkThreadFunction()
        {
            try
            {
                while (IsVirtualRunning)
                {


                    ValuePower = BikeData.GetPower();
                    ValueSpeed = BikeData.GetSpeed();
                    ValueHeartRate = BikeData.GetHeartRate();

                    Console.WriteLine($"We are biking with the values  : {ValuePower} W , {ValueSpeed} m/s, {ValueHeartRate} bpm");
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                // log errors
            }
        }

        private void StopVirtual_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Stop geklikt!");
            IsVirtualRunning = false;

        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ValuePower = (int)virtualResistanceValue.Value;

        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ValueSpeed = (int)VirtualSpeedValue.Value;

        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ValueHeartRate = (int)VirtualHeartRateValue.Value;

        }

        private void StopFysical_Click(object sender, EventArgs e)
        {
            IsFysicalRunning = false;
            Console.WriteLine("Stop butten clicked!!");
           
        }

        private void StartFysical_Click(object sender, EventArgs e)
        {
            IsFysicalRunning = true;
            System.Threading.Thread thread2 = new System.Threading.Thread(new ThreadStart(WorkThreadFunction2Async));
            thread2.Start();

        }

        public async void WorkThreadFunction2Async()
        {
            try
            {

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
                    //Console.WriteLine(System.IO.File.ReadAllText(@"BikeBluetoothName.txt"));
                    errorCode = errorCode = await bleBike.OpenDevice("Avans Bike 1B44");
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
                    errorCode = await bleHeart.OpenDevice(System.IO.File.ReadAllText("Avans Bike 1B44"));


                    await bleHeart.SetService("HeartRate");

                    bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
                    await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");

                    sendResistance();

                    Console.Read();

                }
            }
            catch (Exception ex)
            {
                // log errors
            }
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

        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            if (IsFysicalRunning)
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
                }else if (e.ServiceName == "00002a37-0000-1000-8000-00805f9b34fb")
                {
                    Console.WriteLine($"\n\tHeartRate: {e.Data[1]}bpm");
                }

                //Console.WriteLine("Received from {0}: {1}, {2}", e.ServiceName,
                //    BitConverter.ToString(e.Data).Replace("-", " "),
                //    Encoding.UTF8.GetString(e.Data));
            }
        }

        private void fysicalResistaceValue_ValueChanged(object sender, EventArgs e)
        {
            sendResistance();
        }
    }
}
