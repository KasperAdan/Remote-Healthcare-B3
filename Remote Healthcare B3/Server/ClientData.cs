using System;
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

        public void AddData(string speed, string heartRate, string resistance)
        {
            int? speedData = null;
            int? heartRateData = null;
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
            if (resistance != null || resistance.Trim() != string.Empty)
            {
                resistanceData = int.Parse(speed);
            }
            #endregion

            this.data.Add(new int?[] { speedData, heartRateData, resistanceData });
        }

        public void PrintData()
        {
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"Measurement {i + 1}:   Speed: {data[i][0]}   HeartRate: {data[i][1]}   Resistance: {data[i][2]}");
            }
        }
    }
}