using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avans.TI.BLE;


namespace FietsSimulatorGUI
{
    public partial class BikeSimulator : Form
    {

        public int ValuePower { get; set; }
        public int ValueSpeed { get; set; }
        public int ValueHeartRate { get; set; }

        public Boolean StartButtonClicked { get; set; }
        public Boolean StartButtonClickedv2 { get; set; }

        public int RandomNumber1 { get; set; }
        public int RandomNumber2 { get; set; }
        public int RandomNumber3 { get; set; }

        public Random random { get; set; }


        public int Stroom { get; set; }

        public BikeData BikeData { get; set; }


        public BikeSimulator()
        {
            InitializeComponent();
            ValuePower = 10;
            ValueSpeed = 10;
            ValueHeartRate = 50;
            StartButtonClicked = false;
            StartButtonClickedv2 = false;
            random = new Random();
            BikeData = new BikeData(ValueSpeed, ValueHeartRate, ValuePower, 10);

        }

   

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start geklikt!");
            Console.WriteLine($"Waardes zijn: {ValuePower}, {ValueSpeed}, {ValueHeartRate}");


            ValuePower = (int)numericUpDown1.Value;
            ValueSpeed = (int)numericUpDown2.Value;
            ValueHeartRate = (int)numericUpDown3.Value;

            BikeData.Power = ValuePower;
            BikeData.Speed = ValueSpeed;
            BikeData.HeartRate = ValueHeartRate;

            StartButtonClicked = true;
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
        }

        public void WorkThreadFunction()
        {
            try
            {
                while (StartButtonClicked)
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

        private void button2_Click(object sender, EventArgs e)
        {

            Console.WriteLine("Stop geklikt!");
            StartButtonClicked = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ValuePower = (int)numericUpDown1.Value;

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ValueSpeed = (int)numericUpDown2.Value;

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ValueHeartRate = (int)numericUpDown3.Value;

        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            StartButtonClickedv2 = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            System.Threading.Thread thread2 = new System.Threading.Thread(new ThreadStart(WorkThreadFunction2Async));
            thread2.Start();

        }

        public async void WorkThreadFunction2Async()
        {
            try
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

            
            catch (Exception ex)
            {
                // log errors
            }
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
