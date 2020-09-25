using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsSimulatorGUI
{
    public class BikeData
    {
        public float Speed { get; set; }
        public int HeartRate { get; set; }

        public float Resistance { get; set; }

        public int Spread { get; set; }

        Random Random;
        public BikeData(float Speed, int HeartRate,  float Resistance , int Spread)
        {
            this.Speed = Speed;
            this.HeartRate = HeartRate;
            this.Spread = Spread;
            this.Resistance = Resistance;
            this.Random = new Random();
        }

        public int GetHeartRate()
        {
            return HeartRate + this.Random.Next(-Spread, Spread);
        }

        public float GetSpeed()
        {
            return Speed +(this.Random.Next(-Spread, Spread));
        }


        public override string ToString()
        {
            return $"Speed = {Speed}, HeartRate = {HeartRate},Resistance = {Resistance} , Spread = {Spread} ";
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
