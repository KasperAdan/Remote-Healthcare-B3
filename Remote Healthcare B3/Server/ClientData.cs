using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    internal class ClientData
    {
        public List<float?[]> data;
        public List<List<float?[]>> graphs;
        private bool acceptData;
        public ClientData()
        {
            graphs = new List<List<float?[]>>();
            data = new List<float?[]>();
            acceptData = false;
        }

        public void AddData(string speed, string heartRate, string resistance, string time, string day, string month, string year)
        {
            if(data.Count == 23)
            {
                finishGraph();
                startGraph();
            }
            if (!acceptData) { return; }
            float? speedData = null;
            float? heartRateData = null;
            float? resistanceData = null;
            float? timeData = null;
            float? dayData = null;
            float? monthData = null;
            float? yearData = null;

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
            if (day != null || day.Trim() != string.Empty)
            {
                dayData = float.Parse(day);
            }
            if (month != null || month.Trim() != string.Empty)
            {
                monthData = float.Parse(month);
            }
            if (year != null || year.Trim() != string.Empty)
            {
                yearData = float.Parse(year);
            }
            #endregion

            this.data.Add(new float?[] { speedData, heartRateData, resistanceData, timeData, dayData, monthData, yearData });
            Console.WriteLine("Added data!");
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

        public void finishGraph()
        {
            if (!acceptData) { return; }
            graphs.Add(data);
            acceptData = false;
        }

        public void startGraph()
        {
            if (acceptData) { return; }
            data = new List<float?[]>();
            acceptData = true;
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
