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


        private string username;
        private bool loggedIn;

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
            HistoricData = new List<List<float?[]>>();
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

        /*private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);

            byte[] totalBufferArray = new byte[0];

            totalBufferArray = concat(totalBufferArray, buffer, receivedBytes);

            while (totalBuffer.Length > 8)
            {
                int encryptedLength = BitConverter.ToInt32(totalBufferArray, 0);
                int decryptedLength = BitConverter.ToInt32(totalBufferArray, 4);

                if (totalBufferArray.Length >= 8 + encryptedLength)
                {
                    //string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                    byte[] PartialBuffer = totalBufferArray.Skip(8).Take(encryptedLength).ToArray();
                    String Decrypted = Crypting.DecryptStringFromBytes(PartialBuffer);


                    string[] packetData = Regex.Split(Decrypted, "\r\n");
                    HandleData(packetData);
                }
                else
                {
                    break;
                }
            }

            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }*/

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
                            List<float?[]> recentData = JsonConvert.DeserializeObject<List<float?[]>>(packetData[2]);
                            UpdateRecentData(recentData);
                        });
                    }
                    else
                    {
                        if (!packetData[1].Equals(cbSessionClients.SelectedItem.ToString()))
                        {
                            return;
                        }

                        List<float?[]> recentData = JsonConvert.DeserializeObject<List<float?[]>>(packetData[2]);
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
            LVRecentData.Columns.Add("Date");
            LVRecentData.Columns.Add("Time");
            LVRecentData.Columns.Add("Speed");
            LVRecentData.Columns.Add("Heartrate");
            LVRecentData.Columns.Add("Resistance");
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
            LVHistoricData.Columns.Add("Date");
            LVHistoricData.Columns.Add("Time");
            LVHistoricData.Columns.Add("Speed");
            LVHistoricData.Columns.Add("Heartrate");
            LVHistoricData.Columns.Add("Resistance");
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
        }

        public void ChatToAll(string message)
        {
            Write($"chatToAll\r\n{message}");
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

            List<float?[]> selectedGraph = HistoricData[selectedIndex];
            UpdateHistoricData(selectedGraph);
        }
    }
}