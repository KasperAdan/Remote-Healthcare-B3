using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Configuration;

namespace DokterApplicatie
{
    public partial class FormMainView : Form
    {
        private List<string> Clients;
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer;

        private string username;
        private bool loggedIn;

        public FormMainView()
        {
            Clients = new List<string>();
            Connect();

            while (!loggedIn){}

            InitializeComponent();
            getClients();
            cbMessageClient.Items.Add("All clients");
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
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
            while (result != DialogResult.Yes){}
            Write($"DoctorLogin\r\n{loginForm.username}\r\n{loginForm.password}");
        }

        private void ShowLogin(string error)
        {
            FormLogin loginForm = new FormLogin(error);
            var result = loginForm.ShowDialog();
            while (result != DialogResult.Yes){}
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
            string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            totalBuffer += receivedText;

            while (totalBuffer.Contains("\r\n\r\n"))
            {
                string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
                totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
                string[] packetData = Regex.Split(packet, "\r\n");
                handleData(packetData);
            }
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        private void Write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }


        private void handleData(string[] packetData)
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
                    if(packetData[1] == "ok")
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
                    if(packetData[1] != "ok")
                    {
                        return;
                    }
                    Clients = new List<string>();
                    int UserAmount = int.Parse(packetData[2]);
                    for(int i = 0; i < UserAmount; i++)
                    {
                        Console.WriteLine("Got:"+ packetData[i+3]);
                        Clients.Add(packetData[i + 3]);
                    }
                    updateComboBoxes();
                    break;
                case"AddClient":
                    Console.WriteLine("AddClient: " + packetData[1]);
                    username = packetData[1];
                    Clients.Add(username);
                    updateComboBoxes();
                    break;
                default:
                    //Console.WriteLine("Did not understand: " + packetData[0]);
                    break;

            }

        }

        private void updateComboBoxes()
        {
            cbMessageClient.Items.Clear();
            foreach(string username in Clients)
            {
                cbMessageClient.Items.Add(username);
            }
            cbMessageClient.Items.Add("All clients");
            cbMessageClient.Refresh();

            cbSessionClients.Items.Clear();
            foreach (string username in Clients)
            {
                cbSessionClients.Items.Add(username);
            }
            cbSessionClients.Refresh();
        }

        private void tabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
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

        public void directMessage(string username, string message)
        {
            Write($"directMessage\r\n{username}\r\n{message}");
        }

        public void chatToAll(string message)
        {
            Write($"chatToAll\r\n{message}");
        }

        private void btnStartSession_Click(object sender, EventArgs e)
        {
            //dictionary to connect user with tab
            Write($"StartTraining\r\njkb");
        }

        private void btnStopSession_Click(object sender, EventArgs e)
        {
            //dictionary to connect user with tab
            Write($"StopTraining\r\njkb");
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Object selectedItem = cbMessageClient.SelectedItem;
            if(selectedItem.ToString().Equals("All clients"))
            {
                chatToAll(tbMessage.Text);
            }
            else
            {
                directMessage(selectedItem.ToString(), tbMessage.Text);
            }
        }

        private void getClients()
        {
            Write("GetClients");
        }
    }
}
