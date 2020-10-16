using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DokterApplicatie
{
    public partial class FormMainView : Form
    {
        private List<string> Clients;
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        //private string totalBuffer;
        private byte[] totalBuffer = new byte[0];
        private List<string> AllMessages;


        private string username;
        private bool loggedIn;
        private string selectedHistoricRadiobutton = "Speed";
        private string selectedRealtimeRadiobutton = "Speed";
        private List<float?[]> selectedGraph;
        private List<float?[]> recentData;

        private List<List<float?[]>> HistoricData;

        public FormMainView()
        {
            Clients = new List<string>();
            Connect();

            while (!loggedIn) //Het programma loopt hier vast. In de ClientHandler komt het bericht niet binnen!
            {
            }

            InitializeComponent();
            ListViewRecentDataInit();
            ListViewHistoricDataInit();
            GetClients();
            ResistaneSlider.ValueChanged += ResistaneSlider_ValueChanged;
            HistoricData = new List<List<float?[]>>();
            AllMessages = new List<string>();
            tabControl1.DrawItem += new DrawItemEventHandler(TabControl1_DrawItem);
        }

        public void Connect()
        {
            this.client = new TcpClient();
            this.client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            ShowLogin();
        }

        private void ShowLogin()
        {
            FormLogin loginForm = new FormLogin();
            var result = loginForm.ShowDialog();
            while (result != DialogResult.Yes)
            {
            }

            Write($"DoctorLogin\r\n{loginForm.username}\r\n{loginForm.password}");
            Debug.WriteLine("Doctorlogin send to server...");
        }

        private void ShowLogin(string error)
        {
            FormLogin loginForm = new FormLogin(error);
            var result = loginForm.ShowDialog();
            while (result != DialogResult.Yes)
            {
            }

            this.username = loginForm.username;
            Write($"DoctorLogin\r\n{loginForm.username}\r\n{loginForm.password}");
        }

        private void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);
            totalBuffer = concat(totalBuffer, buffer, receivedBytes);

            while (totalBuffer.Length > 8)
            {
                int encryptedLength = BitConverter.ToInt32(totalBuffer, 0); //waarom is deze lengte zo verschrikkelijk groot?
                int decryptedLength = BitConverter.ToInt32(totalBuffer, 4); //waarom is deze lengte negatief?

                if (totalBuffer.Length >= 8 + encryptedLength)
                {
                    byte[] PartialBuffer = totalBuffer.Skip(8).Take(encryptedLength).ToArray();
                    string Decrypted = Crypting.DecryptStringFromBytes(PartialBuffer);

                    string[] packetData = Regex.Split(Decrypted, "\r\n");
                    HandleData(packetData);
                    totalBuffer = totalBuffer.Skip(encryptedLength + 8).Take(totalBuffer.Length - encryptedLength - 8).ToArray();
                }
                else
                {
                    break;
                }
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private byte[] concat(byte[] b1, byte[] b2, int b2count)
        {
            byte[] total = new byte[b1.Length + b2count];
            Buffer.BlockCopy(b1, 0, total, 0, b1.Length);
            Buffer.BlockCopy(b2, 0, total, b1.Length, b2count);
            return total;
        }

        private void Write(string data)
        {
            var dataAsBytes = Encoding.ASCII.GetBytes(data + "\r\n\r\n");

            var dataStringEncrypted = Crypting.EncryptStringToBytes(data + "\r\n\r\n");


            Debug.WriteLine("Non encrypted.. " + Encoding.ASCII.GetString(dataAsBytes));

            Debug.WriteLine("Encrypted " + Encoding.ASCII.GetString(dataStringEncrypted));

            stream.Write(BitConverter.GetBytes(dataStringEncrypted.Length), 0, 4);
            stream.Write(BitConverter.GetBytes(dataAsBytes.Length), 0, 4);

            stream.Write(dataStringEncrypted, 0, dataStringEncrypted.Length);

            stream.Flush();
        }


        private void HandleData(string[] packetData)
        {
            //Console.WriteLine($"Packet ontvangen: {packetData[0]}");

            switch (packetData[0])
            {
                case "DoctorLogin":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Connected");
                        loggedIn = true;
                    }
                    else if (packetData[1] == "error")
                    {
                        ShowLogin(packetData[2]);
                        Console.WriteLine(packetData[2]);
                    }

                    break;
                case "data":
                    //Console.WriteLine(packetData[1]);
                    break;
                case "chatToAll":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("All clients received message!");
                    }

                    break;
                case "directMessage":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Client received message!");
                    }

                    break;
                case "GetClients":
                    if (packetData[1] != "ok")
                    {
                        return;
                    }

                    Clients = new List<string>();
                    int UserAmount = int.Parse(packetData[2]);
                    for (int i = 0; i < UserAmount; i++)
                    {
                        Console.WriteLine("Got:" + packetData[i + 3]);
                        Clients.Add(packetData[i + 3]);
                    }

                    UpdateComboBoxes();
                    break;
                case "AddClient":
                    Console.WriteLine("AddClient: " + packetData[1]);
                    username = packetData[1];
                    Clients.Add(username);
                    UpdateComboBoxes();
                    break;
                case "StartTraining":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Training started");
                    }

                    break;
                case "StopTraining":
                    if (packetData[1] == "ok")
                    {
                        Console.WriteLine("Training stopped");
                    }

                    break;
                case "RealTimeData":
                    if (cbSessionClients.InvokeRequired)
                    {
                        cbSessionClients.Invoke((MethodInvoker) delegate
                        {
                            if (cbSessionClients.SelectedItem == null ||
                                !packetData[1].Equals(cbSessionClients.SelectedItem.ToString()))
                            {
                                return;
                            }

                            Debug.WriteLine("Got recent data: " + packetData[2]);
                            recentData = JsonConvert.DeserializeObject<List<float?[]>>(packetData[2]);
                            UpdateRecentData(recentData);
                        });
                    }
                    else
                    {
                        if (!packetData[1].Equals(cbSessionClients.SelectedItem.ToString()))
                        {
                            return;
                        }

                        recentData = JsonConvert.DeserializeObject<List<float?[]>>(packetData[2]);
                        UpdateRecentData(recentData);
                    }

                    break;
                case "GetHistoricData":
                    List<List<float?[]>> historicData =
                        JsonConvert.DeserializeObject<List<List<float?[]>>>(packetData[2]);
                    this.HistoricData = historicData;
                    if (cbTime.InvokeRequired)
                    {
                        cbTime.Invoke((MethodInvoker) delegate { updateDateCombobox(); });
                    }
                    else
                    {
                        updateDateCombobox();
                    }


                    break;
                default:
                    Console.WriteLine("Did not understand: " + packetData[0]);
                    break;
            }
        }

        private void ListViewRecentDataInit()
        {
            LVRecentData.View = View.Details;
            LVRecentData.Columns.Add("Date", 100);
            LVRecentData.Columns.Add("Time", 100);
            LVRecentData.Columns.Add("Speed", 100);
            LVRecentData.Columns.Add("Heartrate", 100);
            LVRecentData.Columns.Add("Resistance", 100);
        }

        private void UpdateRecentData(List<float?[]> data)
        {
            LVRecentData.Items.Clear();

            foreach (float?[] dataPoint in data)
            {
                float? totalSeconds = dataPoint[3];
                int hours = (int) totalSeconds / 3600;
                int minutes = ((int) totalSeconds % 3600) / 60;
                int seconds = (int) totalSeconds % 60;
                LVRecentData.Items.Add(new ListViewItem(new string[]
                {
                    $"{dataPoint[4]}-{dataPoint[5]}-{dataPoint[6]}", $"{hours:00}:{minutes:00}:{seconds:00}",
                    dataPoint[0].ToString(), dataPoint[1].ToString(), dataPoint[2].ToString()
                }));
            }

            LVRecentData.Items[LVRecentData.Items.Count - 1].EnsureVisible();

            switch (selectedRealtimeRadiobutton)
            {
                case "Speed":
                    loadSpeedChart(recentData, cRealtimeData);
                    break;
                case "Heartrate":
                    loadHeartRateChart(recentData, cRealtimeData);
                    break;
                case "Resistance":
                    loadResistanceChart(recentData, cRealtimeData);
                    break;
            }
        }

        private void UpdateHistoricData(List<float?[]> graph)
        {
            LVHistoricData.Items.Clear();
            foreach (float?[] dataPoint in graph)
            {
                float? totalSeconds = dataPoint[3];
                int hours = (int) totalSeconds / 3600;
                int minutes = ((int) totalSeconds % 3600) / 60;
                int seconds = (int) totalSeconds % 60;
                LVHistoricData.Items.Add(new ListViewItem(new string[]
                {
                    $"{dataPoint[4]}-{dataPoint[5]}-{dataPoint[6]}", $"{hours:00}:{minutes:00}:{seconds:00}",
                    dataPoint[0].ToString(), dataPoint[1].ToString(), dataPoint[2].ToString()
                }));
            }
        }

        private void ListViewHistoricDataInit()
        {
            LVHistoricData.View = View.Details;
            LVHistoricData.Columns.Add("Date", 100);
            LVHistoricData.Columns.Add("Time", 100);
            LVHistoricData.Columns.Add("Speed", 100);
            LVHistoricData.Columns.Add("Heartrate", 100);
            LVHistoricData.Columns.Add("Resistance", 100);
        }

        private void updateDateCombobox()
        {
            cbTime.Items.Clear();
            foreach (List<float?[]> graph in HistoricData)
            {
                float? totalSeconds = graph[1][3];
                int hours = (int) totalSeconds / 3600;
                int minutes = ((int) totalSeconds % 3600) / 60;
                int seconds = (int) totalSeconds % 60;
                string DateTime = $"{hours}:{minutes}:{seconds} : {graph[1][4]}-{graph[1][5]}-{graph[1][6]}";
                cbTime.Items.Add(DateTime);
            }

            cbTime.Refresh();
        }

        private void UpdateComboBoxes()
        {
            if (cbMessageClient.InvokeRequired)
            {
                cbMessageClient.Invoke((MethodInvoker) delegate
                {
                    cbMessageClient.Items.Clear();
                    foreach (string username in Clients)
                    {
                        cbMessageClient.Items.Add(username);
                    }

                    cbMessageClient.Items.Add("All clients");
                    cbMessageClient.Refresh();
                });
            }
            else
            {
                cbMessageClient.Items.Clear();
                foreach (string username in Clients)
                {
                    cbMessageClient.Items.Add(username);
                }

                cbMessageClient.Items.Add("All clients");
                cbMessageClient.Refresh();
            }

            if (cbSessionClients.InvokeRequired)
            {
                cbSessionClients.Invoke((MethodInvoker) delegate
                {
                    cbSessionClients.Items.Clear();
                    foreach (string username in Clients)
                    {
                        cbSessionClients.Items.Add(username);
                    }

                    cbSessionClients.Refresh();
                });
            }
            else
            {
                cbSessionClients.Items.Clear();
                foreach (string username in Clients)
                {
                    cbSessionClients.Items.Add(username);
                }

                cbSessionClients.Refresh();
            }

            if (cbUsername.InvokeRequired)
            {
                cbUsername.Invoke((MethodInvoker) delegate
                {
                    cbUsername.Items.Clear();
                    foreach (string username in Clients)
                    {
                        cbUsername.Items.Add(username);
                    }

                    cbUsername.Refresh();
                });
            }
            else
            {
                cbUsername.Items.Clear();
                foreach (string username in Clients)
                {
                    cbUsername.Items.Add(username);
                }

                cbUsername.Refresh();
            }
        }

        private void TabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Aqua);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        public void DirectMessage(string username, string message)
        {
            Write($"directMessage\r\n{username}\r\n{message}");
            AllMessages.Add($"> {username} : {message}");
            UpdateSendText();
        }

        public void ChatToAll(string message)
        {
            Write($"chatToAll\r\n{message}");
            AllMessages.Add($"> All clients : {message}");
            UpdateSendText();
        }

        private void BtnStartSession_Click(object sender, EventArgs e)
        {
            //dictionary to connect user with tab
            if (cbSessionClients.SelectedItem == null)
            {
                return;
            }

            string username = cbSessionClients.SelectedItem.ToString();
            StartTraining(username);
        }

        private void BtnStopSession_Click(object sender, EventArgs e)
        {
            //dictionary to connect user with tab
            string username = cbSessionClients.SelectedItem.ToString();
            StopTraining(username);
        }

        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            Object selectedItem = cbMessageClient.SelectedItem;
            if (selectedItem.ToString().Equals("All clients"))
            {
                ChatToAll(tbMessage.Text);
            }
            else
            {
                DirectMessage(selectedItem.ToString(), tbMessage.Text);
            }
            tbMessage.Text = "";
        }

        private void btnGetHistoricData_Click(object sender, EventArgs e)
        {
            string username = cbUsername.SelectedItem.ToString();
            GetHistoricData(username);
        }

        private void GetClients()
        {
            Write("GetClients");
        }

        private void StartTraining(string username)
        {
            Write($"StartTraining\r\n{username}");
            Write($"SetResistance\r\n{username}\r\n{0}");
        }

        private void StopTraining(string username)
        {
            Write($"StopTraining\r\n{username}");
        }

        private void GetHistoricData(string username)
        {
            Write($"GetHistoricData\r\n{username}");
        }

        private void LoadTableButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = cbTime.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }

            selectedGraph = HistoricData[selectedIndex];
            UpdateHistoricData(selectedGraph);

            switch (selectedHistoricRadiobutton)
            {
                case "Speed":
                    loadSpeedChart(selectedGraph, cHistoricData);
                    break;
                case "Heartrate":
                    loadHeartRateChart(selectedGraph, cHistoricData);
                    break;
                case "Resistance":
                    loadResistanceChart(selectedGraph, cHistoricData);
                    break;
            }
        }

        private void UpdateSendText()
        {
            lblAllMessages.Text = "";
            foreach (String text in AllMessages)
            {
                lblAllMessages.Text += text + "\r\n";
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            UpdateSendText();
        }

        private void buttonSetRestance_Click(object sender, EventArgs e)
        {
            int selectedResistance = ResistaneSlider.Value;

            if(selectedResistance <= 100 && selectedResistance >= 0)
            {
                if(cbSessionClients.SelectedItem == null) { return; }
                string selectedUser = cbSessionClients.SelectedItem.ToString();

                if(selectedUser != null)
                {
                    Write($"SetResistance\r\n{selectedUser}\r\n{selectedResistance}");
                }
            }
        }

        private void ResistaneSlider_ValueChanged(object sender, EventArgs e)
        {
            labelSelectedResistance.Text = (ResistaneSlider.Value).ToString();

        }

        private void loadSpeedChart(List<float?[]> graph, Chart chart)
        {
            if(graph == null) { return; }
            chart.Series.Clear();
            var resistanceSeries = chart.ChartAreas[0];

            resistanceSeries.AxisX.IntervalType = DateTimeIntervalType.Number;
            resistanceSeries.AxisX.Minimum = 0;
            resistanceSeries.AxisY.Minimum = 0;
            resistanceSeries.AxisX.Interval = graph.Count/10;
            resistanceSeries.AxisY.Interval = 5;



            chart.Series.Add("Speed");
            chart.Series["Speed"].ChartType = SeriesChartType.Line;
            chart.Series["Speed"].Color = Color.Green;
            chart.Series[0].IsVisibleInLegend = true;

            float? beginSeconds = graph[0][3];

            foreach(float?[] dataPoint in graph)
            {
                float? totalSeconds = dataPoint[3] - beginSeconds;
                int seconds = (int)totalSeconds;
                float? speed = dataPoint[0];
                if(speed != null)
                {
                    chart.Series["Speed"].Points.AddXY(seconds, speed);
                }

            }

        }

        private void loadHeartRateChart(List<float?[]> graph, Chart chart)
        {
            if (graph == null) { return; }
            chart.Series.Clear();
            var resistanceSeries = chart.ChartAreas[0];

            resistanceSeries.AxisX.IntervalType = DateTimeIntervalType.Number;
            resistanceSeries.AxisX.Minimum = 0;
            resistanceSeries.AxisY.Minimum = 50;
            resistanceSeries.AxisX.Interval = graph.Count / 10;
            resistanceSeries.AxisY.Interval = 10;



            chart.Series.Add("HeartRate");
            chart.Series["HeartRate"].ChartType = SeriesChartType.Line;
            chart.Series["HeartRate"].Color = Color.Red;
            chart.Series[0].IsVisibleInLegend = true;

            float? beginSeconds = graph[0][3];

            foreach (float?[] dataPoint in graph)
            {
                float? totalSeconds = dataPoint[3] - beginSeconds;
                int seconds = (int)totalSeconds;
                float? heartRate = dataPoint[1];
                if (heartRate != null)
                {
                    chart.Series["HeartRate"].Points.AddXY(seconds, heartRate);
                }

            }

        }

        private void loadResistanceChart(List<float?[]> graph, Chart chart)
        {
            if (graph == null) { return; }
            chart.Series.Clear();
            var resistanceSeries = chart.ChartAreas[0];

            resistanceSeries.AxisX.IntervalType = DateTimeIntervalType.Number;
            resistanceSeries.AxisX.Minimum = 0;
            resistanceSeries.AxisY.Minimum = 0;
            resistanceSeries.AxisX.Interval = graph.Count / 10;
            resistanceSeries.AxisY.Interval = 10;



            chart.Series.Add("Resistance");
            chart.Series["Resistance"].ChartType = SeriesChartType.Line;
            chart.Series["Resistance"].Color = Color.Blue;
            chart.Series[0].IsVisibleInLegend = true;

            float? beginSeconds = graph[0][3];

            foreach (float?[] dataPoint in graph)
            {
                float? totalSeconds = dataPoint[3] - beginSeconds;
                int seconds = (int)totalSeconds;
                float? resistance = dataPoint[2];
                if (resistance != null)
                {
                    chart.Series["Resistance"].Points.AddXY(seconds, resistance);
                }

            }

        }

        private void rbSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSpeed.Checked)
            {
                selectedHistoricRadiobutton = "Speed";
                loadSpeedChart(selectedGraph, cHistoricData);
            }
            
            
        }

        private void rbHeartRate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHeartRate.Checked)
            {
                selectedHistoricRadiobutton = "Heartrate";
                loadHeartRateChart(selectedGraph, cHistoricData);
            }
        }

        private void rbResistance_CheckedChanged(object sender, EventArgs e)
        {
            if (rbResistance.Checked)
            {
                selectedHistoricRadiobutton = "Resistance";
                loadResistanceChart(selectedGraph, cHistoricData);
            }
        }

        private void rbRealtimeSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRealtimeSpeed.Checked)
            {
                selectedRealtimeRadiobutton = "Speed";
                loadSpeedChart(recentData, cRealtimeData);
            }
        }

        private void rbRealtimeHeartrate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRealtimeHeartrate.Checked)
            {
                selectedRealtimeRadiobutton = "Heartrate";
                loadHeartRateChart(recentData, cRealtimeData);
            }
        }

        private void rbRealtimeResistance_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRealtimeResistance.Checked)
            {
                selectedRealtimeRadiobutton = "Resistance";
                loadResistanceChart(recentData, cRealtimeData);
            }
        }
    }
}