using System;
using System.Collections;
using System.Collections.Generic;

namespace Server
{
    internal class ClientData
    {
        private List<int?[]> data;
        public ClientData()
        {
            data = new List<int?[]>();
        }

        public void AddData(string speed, string heartRate, string power, string resistance)
        {
            int? speedData = null;
            int? heartRateData = null;
            int? powerData = null;
            int? resistanceData = null;

            #region value casting
            if (speed != null || speed.Trim() != string.Empty)
            {
                speedData = int.Parse(speed);
            }
            if (heartRate != null || heartRate.Trim() != string.Empty)
            {
                heartRateData = int.Parse(speed);
            }
            if (power != null || power.Trim() != string.Empty)
            {
                powerData = int.Parse(speed);
            }
            if (resistance != null || resistance.Trim() != string.Empty)
            {
                resistanceData = int.Parse(speed);
            }
            #endregion

            this.data.Add(new int?[] { speedData, heartRateData, powerData, resistanceData });
        }

        public void PrintData()
        {
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"Measurement {i+1}:\tSpeed: {data[i][0]}\tHeartRate: {data[i][1]}\tPower: {data[i][2]}\tResistance: {data[i][3]}");
            }
        }
    }
}