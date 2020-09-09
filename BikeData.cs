using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsSimulatorGUI
{
    public class BikeData
    {
        public int Speed { get; set; }
        public int HeartRate { get; set; }

        public int Power { get; set; }

        public int Spread { get; set; }

        Random Random;
        public BikeData(int Speed, int HeartRate,  int Power , int Spread)
        {
            this.Speed = Speed;
            this.HeartRate = HeartRate;
            this.Spread = Spread;
            this.Power = Power;
            this.Random = new Random();
        }

        public int GetHeartRate()
        {
            return HeartRate + this.Random.Next(-Spread, Spread);
        }

        public int GetSpeed()
        {
            return Speed + this.Random.Next(-Spread, Spread);
        }

        public int GetPower()
        {
            return Power + this.Random.Next(-Spread, Spread);
        }



        public string ToString()
        {
            return $"Speed = {Speed}, HeartRate = {HeartRate}, Spread = {Spread} ";
        }

        /*
        public byte[] GetANT()
        {
            byte[] ANTBytes = new byte[13] { 0xa4,0x09 ,0x4e ,0x05 , , , , , , , , , , };
            return ANTBytes;
        }
        */
    }
}
