using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    internal class ClientData
    {
        private List<float?[]> data;
        public ClientData()
        {
            data = new List<float?[]>();
        }

        public void AddData(string speed, string heartRate, string resistance, string time)
        {
            float? speedData = null;
            float? heartRateData = null;
            float? resistanceData = null;
            float? timeData = null;

            #region value casting
            if (speed != null || speed.Trim() != string.Empty)
            {
                speedData = float.Parse(speed);
            }
            if (heartRate != null || heartRate.Trim() != string.Empty)
            {
                heartRateData = float.Parse(heartRate);
            }
            if (resistance != null || resistance.Trim() != string.Empty)
            {
                resistanceData = float.Parse(resistance);
            }
            if (time != null || time.Trim() != string.Empty)
            {
                timeData = float.Parse(time);
            }
            #endregion

            this.data.Add(new float?[] { speedData, heartRateData, resistanceData, timeData });
        }

        public void PrintData()
        {
            for (int i = 0; i < data.Count; i++)
            {
                float? totalSeconds = data[i][3];
                int hours = (int)totalSeconds / 3600;
                int minutes = ((int)totalSeconds % 3600)/60;
                int seconds = (int)totalSeconds % 60;

                //Console.WriteLine($"Measurement {i + 1}: Time: {hours:00}:{minutes:00}:{seconds:00}   Speed: {data[i][0]}   HeartRate: {data[i][1]}   Resistance: {data[i][2]}");
            }
        }
        
        public JObject GetJson()
        {
            JObject historyJson =
                new JObject(
                    new JProperty("data", new JArray(from d in this.data select new JArray(d))));
            return historyJson;
        }
    }
}
