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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Monitoring = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.Session1 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Session2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.History = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.Chat = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.Session = new System.Windows.Forms.TabPage();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.TabPage();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStartSession = new System.Windows.Forms.Button();
            this.cbSessionClients = new System.Windows.Forms.ComboBox();
            this.btnStopSession = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.cbMessageClient = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.Monitoring.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.Session1.SuspendLayout();
            this.Session2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.History.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.Chat.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.Session.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
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
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(799, 450);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // Monitoring
            // 
            this.Monitoring.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Monitoring.BackgroundImage")));
            this.Monitoring.Controls.Add(this.btnStopSession);
            this.Monitoring.Controls.Add(this.cbSessionClients);
            this.Monitoring.Controls.Add(this.btnStartSession);
            this.Monitoring.Controls.Add(this.pictureBox1);
            this.Monitoring.Controls.Add(this.tabControl2);
            this.Monitoring.Location = new System.Drawing.Point(104, 4);
            this.Monitoring.Name = "Monitoring";
            this.Monitoring.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Monitoring.Size = new System.Drawing.Size(691, 442);
            this.Monitoring.TabIndex = 0;
            this.Monitoring.Text = "Monitoring";
            this.Monitoring.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.Session1);
            this.tabControl2.Controls.Add(this.Session2);
            this.tabControl2.Location = new System.Drawing.Point(147, 119);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(516, 315);
            this.tabControl2.TabIndex = 0;
            // 
            // Session1
            // 
            this.Session1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Session1.BackgroundImage")));
            this.Session1.Controls.Add(this.listBox1);
            this.Session1.Location = new System.Drawing.Point(4, 22);
            this.Session1.Name = "Session1";
            this.Session1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Session1.Size = new System.Drawing.Size(508, 289);
            this.Session1.TabIndex = 0;
            this.Session1.Text = "Session 1";
            this.Session1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 6);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(73, 108);
            this.listBox1.TabIndex = 0;
            // 
            // Session2
            // 
            this.Session2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Session2.BackgroundImage")));
            this.Session2.Controls.Add(this.pictureBox2);
            this.Session2.Controls.Add(this.listBox2);
            this.Session2.Location = new System.Drawing.Point(4, 22);
            this.Session2.Name = "Session2";
            this.Session2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Session2.Size = new System.Drawing.Size(508, 289);
            this.Session2.TabIndex = 1;
            this.Session2.Text = "Session 2";
            this.Session2.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(281, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(102, 99);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(36, 118);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(73, 108);
            this.listBox2.TabIndex = 1;
            // 
            // History
            // 
            this.History.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("History.BackgroundImage")));
            this.History.Controls.Add(this.pictureBox3);
            this.History.Location = new System.Drawing.Point(104, 4);
            this.History.Name = "History";
            this.History.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.History.Size = new System.Drawing.Size(691, 442);
            this.History.TabIndex = 1;
            this.History.Text = "History";
            this.History.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(294, 8);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(107, 106);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // Chat
            // 
            this.Chat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Chat.BackgroundImage")));
            this.Chat.Controls.Add(this.cbMessageClient);
            this.Chat.Controls.Add(this.tbMessage);
            this.Chat.Controls.Add(this.btnSendMessage);
            this.Chat.Controls.Add(this.pictureBox4);
            this.Chat.Controls.Add(this.tabControl3);
            this.Chat.Location = new System.Drawing.Point(104, 4);
            this.Chat.Name = "Chat";
            this.Chat.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Chat.Size = new System.Drawing.Size(691, 442);
            this.Chat.TabIndex = 2;
            this.Chat.Text = "Chat";
            this.Chat.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage1);
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Location = new System.Drawing.Point(267, 122);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(418, 317);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(410, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(671, 404);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(282, 6);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(108, 101);
            this.pictureBox7.TabIndex = 1;
            this.pictureBox7.TabStop = false;
            // 
            // Session
            // 
            this.Session.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Session.BackgroundImage")));
            this.Session.Controls.Add(this.pictureBox5);
            this.Session.Location = new System.Drawing.Point(104, 4);
            this.Session.Name = "Session";
            this.Session.Size = new System.Drawing.Size(691, 442);
            this.Session.TabIndex = 3;
            this.Session.Text = "Session";
            this.Session.UseVisualStyleBackColor = true;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(294, 8);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(106, 103);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // Settings
            // 
            this.Settings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Settings.BackgroundImage")));
            this.Settings.Controls.Add(this.pictureBox6);
            this.Settings.Location = new System.Drawing.Point(104, 4);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(691, 442);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(294, 8);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(106, 103);
            this.pictureBox6.TabIndex = 1;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(102, 99);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnStartSession
            // 
            this.btnStartSession.Location = new System.Drawing.Point(239, 6);
            this.btnStartSession.Name = "btnStartSession";
            this.btnStartSession.Size = new System.Drawing.Size(80, 23);
            this.btnStartSession.TabIndex = 6;
            this.btnStartSession.Text = "Start session";
            this.btnStartSession.UseVisualStyleBackColor = true;
            this.btnStartSession.Click += new System.EventHandler(this.BtnStartSession_Click);
            // 
            // cbSessionClients
            // 
            this.cbSessionClients.FormattingEnabled = true;
            this.cbSessionClients.Location = new System.Drawing.Point(111, 8);
            this.cbSessionClients.Name = "cbSessionClients";
            this.cbSessionClients.Size = new System.Drawing.Size(121, 21);
            this.cbSessionClients.TabIndex = 7;
            // 
            // btnStopSession
            // 
            this.btnStopSession.Location = new System.Drawing.Point(325, 6);
            this.btnStopSession.Name = "btnStopSession";
            this.btnStopSession.Size = new System.Drawing.Size(80, 23);
            this.btnStopSession.TabIndex = 8;
            this.btnStopSession.Text = "Stop session";
            this.btnStopSession.UseVisualStyleBackColor = true;
            this.btnStopSession.Click += new System.EventHandler(this.BtnStopSession_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(108, 101);
            this.pictureBox4.TabIndex = 2;
            this.pictureBox4.TabStop = false;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(267, 6);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(109, 23);
            this.btnSendMessage.TabIndex = 3;
            this.btnSendMessage.Text = "Send message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.BtnSendMessage_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(117, 35);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(259, 69);
            this.tbMessage.TabIndex = 4;
            // 
            // cbMessageClient
            // 
            this.cbMessageClient.FormattingEnabled = true;
            this.cbMessageClient.Location = new System.Drawing.Point(117, 8);
            this.cbMessageClient.Name = "cbMessageClient";
            this.cbMessageClient.Size = new System.Drawing.Size(121, 21);
            this.cbMessageClient.TabIndex = 5;
            // 
            // FormMainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMainView";
            this.Text = "Form2";
            this.tabControl1.ResumeLayout(false);
            this.Monitoring.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.Session1.ResumeLayout(false);
            this.Session2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.History.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.Chat.ResumeLayout(false);
            this.Chat.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.Session.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.Settings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Monitoring;
        private System.Windows.Forms.TabPage History;
        private System.Windows.Forms.TabPage Chat;
        private System.Windows.Forms.TabPage Session;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage Session1;
        private System.Windows.Forms.TabPage Session2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Button btnStopSession;
        private System.Windows.Forms.ComboBox cbSessionClients;
        private System.Windows.Forms.Button btnStartSession;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cbMessageClient;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}