namespace DokterApplicatie
{
    partial class FormMainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainView));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Monitoring = new System.Windows.Forms.TabPage();
            this.cRealtimeData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rbRealtimeResistance = new System.Windows.Forms.RadioButton();
            this.rbRealtimeHeartrate = new System.Windows.Forms.RadioButton();
            this.rbRealtimeSpeed = new System.Windows.Forms.RadioButton();
            this.labelSelectedResistance = new System.Windows.Forms.Label();
            this.labelMaxResistance = new System.Windows.Forms.Label();
            this.labelMinResistance = new System.Windows.Forms.Label();
            this.buttonSetRestance = new System.Windows.Forms.Button();
            this.ResistaneSlider = new System.Windows.Forms.TrackBar();
            this.LVRecentData = new System.Windows.Forms.ListView();
            this.btnStopSession = new System.Windows.Forms.Button();
            this.cbSessionClients = new System.Windows.Forms.ComboBox();
            this.btnStartSession = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.History = new System.Windows.Forms.TabPage();
            this.rbResistance = new System.Windows.Forms.RadioButton();
            this.rbHeartRate = new System.Windows.Forms.RadioButton();
            this.rbSpeed = new System.Windows.Forms.RadioButton();
            this.cHistoricData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LoadTableButton = new System.Windows.Forms.Button();
            this.LVHistoricData = new System.Windows.Forms.ListView();
            this.btnGetHistoricData = new System.Windows.Forms.Button();
            this.cbTime = new System.Windows.Forms.ComboBox();
            this.cbUsername = new System.Windows.Forms.ComboBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.Chat = new System.Windows.Forms.TabPage();
            this.lvAllMessages = new System.Windows.Forms.ListView();
            this.cbMessageClient = new System.Windows.Forms.ComboBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.Session = new System.Windows.Forms.TabPage();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.TabPage();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.Monitoring.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cRealtimeData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResistaneSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.History.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHistoricData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.Chat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.Session.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.Monitoring);
            this.tabControl1.Controls.Add(this.History);
            this.tabControl1.Controls.Add(this.Chat);
            this.tabControl1.Controls.Add(this.Session);
            this.tabControl1.Controls.Add(this.Settings);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.ItemSize = new System.Drawing.Size(30, 100);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1065, 554);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Click += new System.EventHandler(this.TabControl1_Click);
            // 
            // Monitoring
            // 
            this.Monitoring.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Monitoring.BackgroundImage")));
            this.Monitoring.Controls.Add(this.cRealtimeData);
            this.Monitoring.Controls.Add(this.rbRealtimeResistance);
            this.Monitoring.Controls.Add(this.rbRealtimeHeartrate);
            this.Monitoring.Controls.Add(this.rbRealtimeSpeed);
            this.Monitoring.Controls.Add(this.labelSelectedResistance);
            this.Monitoring.Controls.Add(this.labelMaxResistance);
            this.Monitoring.Controls.Add(this.labelMinResistance);
            this.Monitoring.Controls.Add(this.buttonSetRestance);
            this.Monitoring.Controls.Add(this.ResistaneSlider);
            this.Monitoring.Controls.Add(this.LVRecentData);
            this.Monitoring.Controls.Add(this.btnStopSession);
            this.Monitoring.Controls.Add(this.cbSessionClients);
            this.Monitoring.Controls.Add(this.btnStartSession);
            this.Monitoring.Controls.Add(this.pictureBox1);
            this.Monitoring.Location = new System.Drawing.Point(104, 4);
            this.Monitoring.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Monitoring.Name = "Monitoring";
            this.Monitoring.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Monitoring.Size = new System.Drawing.Size(957, 546);
            this.Monitoring.TabIndex = 0;
            this.Monitoring.Text = "Monitoring";
            this.Monitoring.UseVisualStyleBackColor = true;
            // 
            // cRealtimeData
            // 
            chartArea3.Name = "ChartArea1";
            this.cRealtimeData.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.cRealtimeData.Legends.Add(legend3);
            this.cRealtimeData.Location = new System.Drawing.Point(221, 64);
            this.cRealtimeData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cRealtimeData.Name = "cRealtimeData";
            this.cRealtimeData.Size = new System.Drawing.Size(689, 289);
            this.cRealtimeData.TabIndex = 18;
            this.cRealtimeData.Text = "chart1";
            // 
            // rbRealtimeResistance
            // 
            this.rbRealtimeResistance.AutoSize = true;
            this.rbRealtimeResistance.Location = new System.Drawing.Point(421, 41);
            this.rbRealtimeResistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbRealtimeResistance.Name = "rbRealtimeResistance";
            this.rbRealtimeResistance.Size = new System.Drawing.Size(99, 21);
            this.rbRealtimeResistance.TabIndex = 17;
            this.rbRealtimeResistance.Text = "Resistance";
            this.rbRealtimeResistance.UseVisualStyleBackColor = true;
            this.rbRealtimeResistance.CheckedChanged += new System.EventHandler(this.RbRealtimeResistance_CheckedChanged);
            // 
            // rbRealtimeHeartrate
            // 
            this.rbRealtimeHeartrate.AutoSize = true;
            this.rbRealtimeHeartrate.Location = new System.Drawing.Point(321, 41);
            this.rbRealtimeHeartrate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbRealtimeHeartrate.Name = "rbRealtimeHeartrate";
            this.rbRealtimeHeartrate.Size = new System.Drawing.Size(89, 21);
            this.rbRealtimeHeartrate.TabIndex = 16;
            this.rbRealtimeHeartrate.Text = "Heartrate";
            this.rbRealtimeHeartrate.UseVisualStyleBackColor = true;
            this.rbRealtimeHeartrate.CheckedChanged += new System.EventHandler(this.RbRealtimeHeartrate_CheckedChanged);
            // 
            // rbRealtimeSpeed
            // 
            this.rbRealtimeSpeed.AutoSize = true;
            this.rbRealtimeSpeed.Checked = true;
            this.rbRealtimeSpeed.Location = new System.Drawing.Point(221, 41);
            this.rbRealtimeSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbRealtimeSpeed.Name = "rbRealtimeSpeed";
            this.rbRealtimeSpeed.Size = new System.Drawing.Size(70, 21);
            this.rbRealtimeSpeed.TabIndex = 15;
            this.rbRealtimeSpeed.TabStop = true;
            this.rbRealtimeSpeed.Text = "Speed";
            this.rbRealtimeSpeed.UseVisualStyleBackColor = true;
            this.rbRealtimeSpeed.CheckedChanged += new System.EventHandler(this.RbRealtimeSpeed_CheckedChanged);
            // 
            // labelSelectedResistance
            // 
            this.labelSelectedResistance.AutoSize = true;
            this.labelSelectedResistance.Location = new System.Drawing.Point(97, 270);
            this.labelSelectedResistance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelectedResistance.Name = "labelSelectedResistance";
            this.labelSelectedResistance.Size = new System.Drawing.Size(16, 17);
            this.labelSelectedResistance.TabIndex = 14;
            this.labelSelectedResistance.Text = "0";
            // 
            // labelMaxResistance
            // 
            this.labelMaxResistance.AutoSize = true;
            this.labelMaxResistance.Location = new System.Drawing.Point(180, 270);
            this.labelMaxResistance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMaxResistance.Name = "labelMaxResistance";
            this.labelMaxResistance.Size = new System.Drawing.Size(32, 17);
            this.labelMaxResistance.TabIndex = 13;
            this.labelMaxResistance.Text = "100";
            // 
            // labelMinResistance
            // 
            this.labelMinResistance.AutoSize = true;
            this.labelMinResistance.Location = new System.Drawing.Point(16, 270);
            this.labelMinResistance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinResistance.Name = "labelMinResistance";
            this.labelMinResistance.Size = new System.Drawing.Size(16, 17);
            this.labelMinResistance.TabIndex = 12;
            this.labelMinResistance.Text = "0";
            // 
            // buttonSetRestance
            // 
            this.buttonSetRestance.Location = new System.Drawing.Point(77, 308);
            this.buttonSetRestance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSetRestance.Name = "buttonSetRestance";
            this.buttonSetRestance.Size = new System.Drawing.Size(136, 28);
            this.buttonSetRestance.TabIndex = 11;
            this.buttonSetRestance.Text = "Set resistance";
            this.buttonSetRestance.UseVisualStyleBackColor = true;
            this.buttonSetRestance.Click += new System.EventHandler(this.ButtonSetRestance_Click);
            // 
            // ResistaneSlider
            // 
            this.ResistaneSlider.Location = new System.Drawing.Point(4, 210);
            this.ResistaneSlider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ResistaneSlider.Maximum = 100;
            this.ResistaneSlider.Name = "ResistaneSlider";
            this.ResistaneSlider.Size = new System.Drawing.Size(209, 56);
            this.ResistaneSlider.TabIndex = 10;
            // 
            // LVRecentData
            // 
            this.LVRecentData.HideSelection = false;
            this.LVRecentData.Location = new System.Drawing.Point(221, 361);
            this.LVRecentData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LVRecentData.Name = "LVRecentData";
            this.LVRecentData.Size = new System.Drawing.Size(688, 173);
            this.LVRecentData.TabIndex = 9;
            this.LVRecentData.UseCompatibleStateImageBehavior = false;
            // 
            // btnStopSession
            // 
            this.btnStopSession.BackColor = System.Drawing.Color.Red;
            this.btnStopSession.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStopSession.Location = new System.Drawing.Point(505, 7);
            this.btnStopSession.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStopSession.Name = "btnStopSession";
            this.btnStopSession.Size = new System.Drawing.Size(136, 28);
            this.btnStopSession.TabIndex = 8;
            this.btnStopSession.Text = "Stop session";
            this.btnStopSession.UseVisualStyleBackColor = false;
            this.btnStopSession.Click += new System.EventHandler(this.BtnStopSession_Click);
            // 
            // cbSessionClients
            // 
            this.cbSessionClients.FormattingEnabled = true;
            this.cbSessionClients.Location = new System.Drawing.Point(221, 7);
            this.cbSessionClients.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSessionClients.Name = "cbSessionClients";
            this.cbSessionClients.Size = new System.Drawing.Size(160, 24);
            this.cbSessionClients.TabIndex = 7;
            this.cbSessionClients.Text = "Choose username: ";
            // 
            // btnStartSession
            // 
            this.btnStartSession.Location = new System.Drawing.Point(391, 7);
            this.btnStartSession.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartSession.Name = "btnStartSession";
            this.btnStartSession.Size = new System.Drawing.Size(107, 28);
            this.btnStartSession.TabIndex = 6;
            this.btnStartSession.Text = "Start session";
            this.btnStartSession.UseVisualStyleBackColor = true;
            this.btnStartSession.Click += new System.EventHandler(this.BtnStartSession_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(209, 199);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // History
            // 
            this.History.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("History.BackgroundImage")));
            this.History.Controls.Add(this.rbResistance);
            this.History.Controls.Add(this.rbHeartRate);
            this.History.Controls.Add(this.rbSpeed);
            this.History.Controls.Add(this.cHistoricData);
            this.History.Controls.Add(this.LoadTableButton);
            this.History.Controls.Add(this.LVHistoricData);
            this.History.Controls.Add(this.btnGetHistoricData);
            this.History.Controls.Add(this.cbTime);
            this.History.Controls.Add(this.cbUsername);
            this.History.Controls.Add(this.pictureBox3);
            this.History.Location = new System.Drawing.Point(104, 4);
            this.History.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.History.Name = "History";
            this.History.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.History.Size = new System.Drawing.Size(957, 546);
            this.History.TabIndex = 1;
            this.History.Text = "History";
            this.History.UseVisualStyleBackColor = true;
            // 
            // rbResistance
            // 
            this.rbResistance.AutoSize = true;
            this.rbResistance.Location = new System.Drawing.Point(408, 41);
            this.rbResistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbResistance.Name = "rbResistance";
            this.rbResistance.Size = new System.Drawing.Size(99, 21);
            this.rbResistance.TabIndex = 9;
            this.rbResistance.Text = "Resistance";
            this.rbResistance.UseVisualStyleBackColor = true;
            this.rbResistance.CheckedChanged += new System.EventHandler(this.RbResistance_CheckedChanged);
            // 
            // rbHeartRate
            // 
            this.rbHeartRate.AutoSize = true;
            this.rbHeartRate.Location = new System.Drawing.Point(308, 41);
            this.rbHeartRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbHeartRate.Name = "rbHeartRate";
            this.rbHeartRate.Size = new System.Drawing.Size(89, 21);
            this.rbHeartRate.TabIndex = 8;
            this.rbHeartRate.Text = "Heartrate";
            this.rbHeartRate.UseVisualStyleBackColor = true;
            this.rbHeartRate.CheckedChanged += new System.EventHandler(this.RbHeartRate_CheckedChanged);
            // 
            // rbSpeed
            // 
            this.rbSpeed.AutoSize = true;
            this.rbSpeed.Checked = true;
            this.rbSpeed.Location = new System.Drawing.Point(225, 41);
            this.rbSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSpeed.Name = "rbSpeed";
            this.rbSpeed.Size = new System.Drawing.Size(70, 21);
            this.rbSpeed.TabIndex = 7;
            this.rbSpeed.TabStop = true;
            this.rbSpeed.Text = "Speed";
            this.rbSpeed.UseVisualStyleBackColor = true;
            this.rbSpeed.CheckedChanged += new System.EventHandler(this.RbSpeed_CheckedChanged);
            // 
            // cHistoricData
            // 
            chartArea4.Name = "ChartArea1";
            this.cHistoricData.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.cHistoricData.Legends.Add(legend4);
            this.cHistoricData.Location = new System.Drawing.Point(225, 69);
            this.cHistoricData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cHistoricData.Name = "cHistoricData";
            this.cHistoricData.Size = new System.Drawing.Size(685, 306);
            this.cHistoricData.TabIndex = 6;
            this.cHistoricData.Text = "chart1";
            // 
            // LoadTableButton
            // 
            this.LoadTableButton.Location = new System.Drawing.Point(707, 5);
            this.LoadTableButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LoadTableButton.Name = "LoadTableButton";
            this.LoadTableButton.Size = new System.Drawing.Size(100, 28);
            this.LoadTableButton.TabIndex = 5;
            this.LoadTableButton.Text = "Load table";
            this.LoadTableButton.UseVisualStyleBackColor = true;
            this.LoadTableButton.Click += new System.EventHandler(this.LoadTableButton_Click);
            // 
            // LVHistoricData
            // 
            this.LVHistoricData.HideSelection = false;
            this.LVHistoricData.Location = new System.Drawing.Point(225, 383);
            this.LVHistoricData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LVHistoricData.Name = "LVHistoricData";
            this.LVHistoricData.Size = new System.Drawing.Size(684, 150);
            this.LVHistoricData.TabIndex = 4;
            this.LVHistoricData.UseCompatibleStateImageBehavior = false;
            // 
            // btnGetHistoricData
            // 
            this.btnGetHistoricData.Location = new System.Drawing.Point(395, 5);
            this.btnGetHistoricData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetHistoricData.Name = "btnGetHistoricData";
            this.btnGetHistoricData.Size = new System.Drawing.Size(100, 28);
            this.btnGetHistoricData.TabIndex = 3;
            this.btnGetHistoricData.Text = "Get";
            this.btnGetHistoricData.UseVisualStyleBackColor = true;
            this.btnGetHistoricData.Click += new System.EventHandler(this.BtnGetHistoricData_Click);
            // 
            // cbTime
            // 
            this.cbTime.FormattingEnabled = true;
            this.cbTime.Location = new System.Drawing.Point(537, 7);
            this.cbTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(160, 24);
            this.cbTime.TabIndex = 2;
            this.cbTime.Text = "Choose dataset:";
            // 
            // cbUsername
            // 
            this.cbUsername.FormattingEnabled = true;
            this.cbUsername.Location = new System.Drawing.Point(225, 7);
            this.cbUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUsername.Name = "cbUsername";
            this.cbUsername.Size = new System.Drawing.Size(160, 24);
            this.cbUsername.TabIndex = 1;
            this.cbUsername.Text = "Choose username:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(8, 7);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(209, 199);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // Chat
            // 
            this.Chat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Chat.BackgroundImage")));
            this.Chat.Controls.Add(this.lvAllMessages);
            this.Chat.Controls.Add(this.cbMessageClient);
            this.Chat.Controls.Add(this.tbMessage);
            this.Chat.Controls.Add(this.btnSendMessage);
            this.Chat.Controls.Add(this.pictureBox4);
            this.Chat.Location = new System.Drawing.Point(104, 4);
            this.Chat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chat.Name = "Chat";
            this.Chat.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Chat.Size = new System.Drawing.Size(957, 546);
            this.Chat.TabIndex = 2;
            this.Chat.Text = "Chat";
            this.Chat.UseVisualStyleBackColor = true;
            // 
            // lvAllMessages
            // 
            this.lvAllMessages.HideSelection = false;
            this.lvAllMessages.Location = new System.Drawing.Point(221, 84);
            this.lvAllMessages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvAllMessages.Name = "lvAllMessages";
            this.lvAllMessages.Size = new System.Drawing.Size(688, 452);
            this.lvAllMessages.TabIndex = 6;
            this.lvAllMessages.UseCompatibleStateImageBehavior = false;
            // 
            // cbMessageClient
            // 
            this.cbMessageClient.FormattingEnabled = true;
            this.cbMessageClient.Location = new System.Drawing.Point(221, 7);
            this.cbMessageClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMessageClient.Name = "cbMessageClient";
            this.cbMessageClient.Size = new System.Drawing.Size(160, 24);
            this.cbMessageClient.TabIndex = 5;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(221, 41);
            this.tbMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(419, 22);
            this.tbMessage.TabIndex = 4;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(391, 5);
            this.btnSendMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(145, 28);
            this.btnSendMessage.TabIndex = 3;
            this.btnSendMessage.Text = "Send message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.BtnSendMessage_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(4, 4);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(209, 199);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 2;
            this.pictureBox4.TabStop = false;
            // 
            // Session
            // 
            this.Session.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Session.BackgroundImage")));
            this.Session.Controls.Add(this.pictureBox5);
            this.Session.Location = new System.Drawing.Point(104, 4);
            this.Session.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Session.Name = "Session";
            this.Session.Size = new System.Drawing.Size(957, 546);
            this.Session.TabIndex = 3;
            this.Session.Text = "Session";
            this.Session.UseVisualStyleBackColor = true;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(392, 10);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(141, 127);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // Settings
            // 
            this.Settings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Settings.BackgroundImage")));
            this.Settings.Controls.Add(this.pictureBox6);
            this.Settings.Location = new System.Drawing.Point(104, 4);
            this.Settings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(957, 546);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(392, 10);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(141, 127);
            this.pictureBox6.TabIndex = 1;
            this.pictureBox6.TabStop = false;
            // 
            // FormMainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMainView";
            this.Text = "Form2";
            this.tabControl1.ResumeLayout(false);
            this.Monitoring.ResumeLayout(false);
            this.Monitoring.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cRealtimeData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResistaneSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.History.ResumeLayout(false);
            this.History.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHistoricData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.Chat.ResumeLayout(false);
            this.Chat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.Session.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.Settings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Monitoring;
        private System.Windows.Forms.TabPage History;
        private System.Windows.Forms.TabPage Chat;
        private System.Windows.Forms.TabPage Session;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button btnStopSession;
        private System.Windows.Forms.ComboBox cbSessionClients;
        private System.Windows.Forms.Button btnStartSession;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cbMessageClient;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.ListView LVRecentData;
        private System.Windows.Forms.ComboBox cbTime;
        private System.Windows.Forms.ComboBox cbUsername;
        private System.Windows.Forms.Button btnGetHistoricData;
        private System.Windows.Forms.ListView LVHistoricData;
        private System.Windows.Forms.Button LoadTableButton;
        private System.Windows.Forms.Button buttonSetRestance;
        private System.Windows.Forms.TrackBar ResistaneSlider;
        private System.Windows.Forms.Label labelSelectedResistance;
        private System.Windows.Forms.Label labelMaxResistance;
        private System.Windows.Forms.DataVisualization.Charting.Chart cHistoricData;
        private System.Windows.Forms.RadioButton rbResistance;
        private System.Windows.Forms.RadioButton rbHeartRate;
        private System.Windows.Forms.RadioButton rbSpeed;
        private System.Windows.Forms.DataVisualization.Charting.Chart cRealtimeData;
        private System.Windows.Forms.RadioButton rbRealtimeResistance;
        private System.Windows.Forms.RadioButton rbRealtimeHeartrate;
        private System.Windows.Forms.RadioButton rbRealtimeSpeed;
        private System.Windows.Forms.Label labelMinResistance;
        private System.Windows.Forms.ListView lvAllMessages;
    }
}