using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    internal class ClientData
    {
        public List<float?[]> Data;
        public List<List<float?[]>> Graphs;
        private bool AcceptData;
        public ClientData()
        {
            Graphs = new List<List<float?[]>>();
            Data = new List<float?[]>();
            AcceptData = false;
        }

        public void AddData(string speed, string heartRate, string resistance, string time, string day, string month, string year)
        {
            /*
            if(data.Count == )
            {
                finishGraph();
                startGraph();
            }
            */
            if (!AcceptData) { return; }
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

            this.Data.Add(new float?[] { speedData, heartRateData, resistanceData, timeData, dayData, monthData, yearData });
            Console.WriteLine("Added data!");
        }

        public void PrintData()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                float? totalSeconds = Data[i][3];
                int hours = (int)totalSeconds / 3600;
                int minutes = ((int)totalSeconds % 3600)/60;
                int seconds = (int)totalSeconds % 60;

                //Console.WriteLine($"Measurement {i + 1}: Time: {hours:00}:{minutes:00}:{seconds:00}   Speed: {data[i][0]}   HeartRate: {data[i][1]}   Resistance: {data[i][2]}");
            }
        }

        public void FinishGraph()
        {
            if (!AcceptData) { return; }
            Graphs.Add(Data);
            AcceptData = false;
        }

        public void StartGraph()
        {
            if (AcceptData) { return; }
            Data = new List<float?[]>();
            AcceptData = true;
        }
        
        public JObject GetJson()
        {
            JObject historyJson =
                new JObject(
                    new JProperty("data", new JArray(from d in this.Data select new JArray(d))));
            return historyJson;
        }
    }
}
