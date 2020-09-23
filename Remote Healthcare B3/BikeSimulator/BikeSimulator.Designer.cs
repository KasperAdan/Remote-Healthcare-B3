namespace FietsSimulatorGUI
{
    partial class BikeSimulator
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
            this.titleApplication = new System.Windows.Forms.Label();
            this.imageBiker = new System.Windows.Forms.PictureBox();
            this.VirtualResistanceLabel = new System.Windows.Forms.Label();
            this.virtualSpeedLabel = new System.Windows.Forms.Label();
            this.VirtualHeartRateLabel = new System.Windows.Forms.Label();
            this.startVirtualButton = new System.Windows.Forms.Button();
            this.virtualResistanceValue = new System.Windows.Forms.NumericUpDown();
            this.VirtualSpeedValue = new System.Windows.Forms.NumericUpDown();
            this.VirtualHeartRateValue = new System.Windows.Forms.NumericUpDown();
            this.stopVirtualButton = new System.Windows.Forms.Button();
            this.stopFysicalButton = new System.Windows.Forms.Button();
            this.startFysicalButton = new System.Windows.Forms.Button();
            this.fysicalResistaceValue = new System.Windows.Forms.NumericUpDown();
            this.fysicalResistaceLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBiker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtualResistanceValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualSpeedValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualHeartRateValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fysicalResistaceValue)).BeginInit();
            this.SuspendLayout();
            // 
            // titleApplication
            // 
            this.titleApplication.AutoSize = true;
            this.titleApplication.Font = new System.Drawing.Font("Lucida Sans Unicode", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleApplication.ForeColor = System.Drawing.SystemColors.ControlText;
            this.titleApplication.Location = new System.Drawing.Point(16, 22);
            this.titleApplication.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleApplication.Name = "titleApplication";
            this.titleApplication.Size = new System.Drawing.Size(741, 98);
            this.titleApplication.TabIndex = 0;
            this.titleApplication.Text = "Fiets Simulator B3";
            // 
            // imageBiker
            // 
            this.imageBiker.Image = global::BikeSimulator.Properties.Resources.FietsIcon;
            this.imageBiker.Location = new System.Drawing.Point(861, -1);
            this.imageBiker.Margin = new System.Windows.Forms.Padding(4);
            this.imageBiker.Name = "imageBiker";
            this.imageBiker.Size = new System.Drawing.Size(321, 149);
            this.imageBiker.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBiker.TabIndex = 1;
            this.imageBiker.TabStop = false;
            // 
            // VirtualResistanceLabel
            // 
            this.VirtualResistanceLabel.AutoSize = true;
            this.VirtualResistanceLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VirtualResistanceLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.VirtualResistanceLabel.Location = new System.Drawing.Point(23, 161);
            this.VirtualResistanceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VirtualResistanceLabel.Name = "VirtualResistanceLabel";
            this.VirtualResistanceLabel.Size = new System.Drawing.Size(333, 57);
            this.VirtualResistanceLabel.TabIndex = 2;
            this.VirtualResistanceLabel.Text = "Weerstand(W)";
            // 
            // virtualSpeedLabel
            // 
            this.virtualSpeedLabel.AutoSize = true;
            this.virtualSpeedLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.virtualSpeedLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.virtualSpeedLabel.Location = new System.Drawing.Point(23, 282);
            this.virtualSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.virtualSpeedLabel.Name = "virtualSpeedLabel";
            this.virtualSpeedLabel.Size = new System.Drawing.Size(341, 57);
            this.virtualSpeedLabel.TabIndex = 3;
            this.virtualSpeedLabel.Text = "Snelheid(m/s)";
            // 
            // VirtualHeartRateLabel
            // 
            this.VirtualHeartRateLabel.AutoSize = true;
            this.VirtualHeartRateLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VirtualHeartRateLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.VirtualHeartRateLabel.Location = new System.Drawing.Point(23, 395);
            this.VirtualHeartRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VirtualHeartRateLabel.Name = "VirtualHeartRateLabel";
            this.VirtualHeartRateLabel.Size = new System.Drawing.Size(350, 57);
            this.VirtualHeartRateLabel.TabIndex = 4;
            this.VirtualHeartRateLabel.Text = "Hartslag(bpm)";
            // 
            // startVirtualButton
            // 
            this.startVirtualButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startVirtualButton.Location = new System.Drawing.Point(287, 522);
            this.startVirtualButton.Margin = new System.Windows.Forms.Padding(4);
            this.startVirtualButton.Name = "startVirtualButton";
            this.startVirtualButton.Size = new System.Drawing.Size(263, 62);
            this.startVirtualButton.TabIndex = 8;
            this.startVirtualButton.Text = "Start virtuele simulatie";
            this.startVirtualButton.UseVisualStyleBackColor = true;
            this.startVirtualButton.Click += new System.EventHandler(this.StartVirtual_Click);
            // 
            // virtualResistanceValue
            // 
            this.virtualResistanceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.virtualResistanceValue.Location = new System.Drawing.Point(33, 220);
            this.virtualResistanceValue.Margin = new System.Windows.Forms.Padding(4);
            this.virtualResistanceValue.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.virtualResistanceValue.Name = "virtualResistanceValue";
            this.virtualResistanceValue.Size = new System.Drawing.Size(160, 26);
            this.virtualResistanceValue.TabIndex = 9;
            this.virtualResistanceValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.virtualResistanceValue.ValueChanged += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // VirtualSpeedValue
            // 
            this.VirtualSpeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VirtualSpeedValue.Location = new System.Drawing.Point(33, 342);
            this.VirtualSpeedValue.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualSpeedValue.Name = "VirtualSpeedValue";
            this.VirtualSpeedValue.Size = new System.Drawing.Size(160, 26);
            this.VirtualSpeedValue.TabIndex = 10;
            this.VirtualSpeedValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.VirtualSpeedValue.ValueChanged += new System.EventHandler(this.NumericUpDown2_ValueChanged);
            // 
            // VirtualHeartRateValue
            // 
            this.VirtualHeartRateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VirtualHeartRateValue.Location = new System.Drawing.Point(33, 466);
            this.VirtualHeartRateValue.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualHeartRateValue.Maximum = new decimal(new int[] {
            228,
            0,
            0,
            0});
            this.VirtualHeartRateValue.Name = "VirtualHeartRateValue";
            this.VirtualHeartRateValue.Size = new System.Drawing.Size(160, 26);
            this.VirtualHeartRateValue.TabIndex = 11;
            this.VirtualHeartRateValue.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.VirtualHeartRateValue.ValueChanged += new System.EventHandler(this.NumericUpDown3_ValueChanged);
            // 
            // stopVirtualButton
            // 
            this.stopVirtualButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopVirtualButton.Location = new System.Drawing.Point(16, 522);
            this.stopVirtualButton.Margin = new System.Windows.Forms.Padding(4);
            this.stopVirtualButton.Name = "stopVirtualButton";
            this.stopVirtualButton.Size = new System.Drawing.Size(263, 62);
            this.stopVirtualButton.TabIndex = 12;
            this.stopVirtualButton.Text = "Stop virtuele simulatie";
            this.stopVirtualButton.UseVisualStyleBackColor = true;
            this.stopVirtualButton.Click += new System.EventHandler(this.StopVirtual_Click);
            // 
            // stopFysicalButton
            // 
            this.stopFysicalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopFysicalButton.Location = new System.Drawing.Point(636, 522);
            this.stopFysicalButton.Margin = new System.Windows.Forms.Padding(4);
            this.stopFysicalButton.Name = "stopFysicalButton";
            this.stopFysicalButton.Size = new System.Drawing.Size(263, 62);
            this.stopFysicalButton.TabIndex = 13;
            this.stopFysicalButton.Text = "Stop fysieke simulatie";
            this.stopFysicalButton.UseVisualStyleBackColor = true;
            this.stopFysicalButton.Click += new System.EventHandler(this.StopFysical_Click);
            // 
            // startFysicalButton
            // 
            this.startFysicalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startFysicalButton.Location = new System.Drawing.Point(907, 522);
            this.startFysicalButton.Margin = new System.Windows.Forms.Padding(4);
            this.startFysicalButton.Name = "startFysicalButton";
            this.startFysicalButton.Size = new System.Drawing.Size(263, 62);
            this.startFysicalButton.TabIndex = 14;
            this.startFysicalButton.Text = "Start fysieke simulatie";
            this.startFysicalButton.UseVisualStyleBackColor = true;
            this.startFysicalButton.Click += new System.EventHandler(this.StartFysical_Click);
            // 
            // fysicalResistaceValue
            // 
            this.fysicalResistaceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fysicalResistaceValue.Location = new System.Drawing.Point(649, 454);
            this.fysicalResistaceValue.Margin = new System.Windows.Forms.Padding(4);
            this.fysicalResistaceValue.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.fysicalResistaceValue.Name = "fysicalResistaceValue";
            this.fysicalResistaceValue.Size = new System.Drawing.Size(160, 26);
            this.fysicalResistaceValue.TabIndex = 16;
            this.fysicalResistaceValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fysicalResistaceValue.ValueChanged += new System.EventHandler(this.fysicalResistaceValue_ValueChanged);
            // 
            // fysicalResistaceLabel
            // 
            this.fysicalResistaceLabel.AutoSize = true;
            this.fysicalResistaceLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fysicalResistaceLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fysicalResistaceLabel.Location = new System.Drawing.Point(639, 395);
            this.fysicalResistaceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fysicalResistaceLabel.Name = "fysicalResistaceLabel";
            this.fysicalResistaceLabel.Size = new System.Drawing.Size(333, 57);
            this.fysicalResistaceLabel.TabIndex = 15;
            this.fysicalResistaceLabel.Text = "Weerstand(W)";
            // 
            // BikeSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 618);
            this.Controls.Add(this.fysicalResistaceValue);
            this.Controls.Add(this.fysicalResistaceLabel);
            this.Controls.Add(this.startFysicalButton);
            this.Controls.Add(this.stopFysicalButton);
            this.Controls.Add(this.stopVirtualButton);
            this.Controls.Add(this.VirtualHeartRateValue);
            this.Controls.Add(this.VirtualSpeedValue);
            this.Controls.Add(this.virtualResistanceValue);
            this.Controls.Add(this.startVirtualButton);
            this.Controls.Add(this.VirtualHeartRateLabel);
            this.Controls.Add(this.virtualSpeedLabel);
            this.Controls.Add(this.VirtualResistanceLabel);
            this.Controls.Add(this.imageBiker);
            this.Controls.Add(this.titleApplication);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BikeSimulator";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imageBiker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtualResistanceValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualSpeedValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualHeartRateValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fysicalResistaceValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleApplication;
        private System.Windows.Forms.PictureBox imageBiker;
        private System.Windows.Forms.Label VirtualResistanceLabel;
        private System.Windows.Forms.Label virtualSpeedLabel;
        private System.Windows.Forms.Label VirtualHeartRateLabel;
        private System.Windows.Forms.Button startVirtualButton;
        private System.Windows.Forms.NumericUpDown virtualResistanceValue;
        private System.Windows.Forms.NumericUpDown VirtualSpeedValue;
        private System.Windows.Forms.NumericUpDown VirtualHeartRateValue;
        private System.Windows.Forms.Button stopVirtualButton;
        private System.Windows.Forms.Button stopFysicalButton;
        private System.Windows.Forms.Button startFysicalButton;
        private System.Windows.Forms.NumericUpDown fysicalResistaceValue;
        private System.Windows.Forms.Label fysicalResistaceLabel;
    }
}

