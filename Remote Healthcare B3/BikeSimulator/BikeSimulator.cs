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

namespace FietsSimulatorGUI
{
    public partial class BikeSimulator : Form
    {

        public int ValuePower { get; set; }
        public int ValueSpeed { get; set; }
        public int ValueHeartRate { get; set; }

        public Boolean StartButtonClicked { get; set; }

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

        public void WorkThreadFunction()
        {
            try
            {
                while(StartButtonClicked) {
                    

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
    }
}
