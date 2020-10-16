namespace DokterApplicatie
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.UsernameBoxImage = new System.Windows.Forms.PictureBox();
            this.PasswordBoxImage = new System.Windows.Forms.PictureBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginLogo = new System.Windows.Forms.PictureBox();
            this.ErrorMessageTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.UsernameBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // UsernameBoxImage
            // 
            this.UsernameBoxImage.Image = ((System.Drawing.Image)(resources.GetObject("UsernameBoxImage.Image")));
            this.UsernameBoxImage.Location = new System.Drawing.Point(33, 276);
            this.UsernameBoxImage.Margin = new System.Windows.Forms.Padding(4);
            this.UsernameBoxImage.Name = "UsernameBoxImage";
            this.UsernameBoxImage.Size = new System.Drawing.Size(475, 68);
            this.UsernameBoxImage.TabIndex = 0;
            this.UsernameBoxImage.TabStop = false;
            // 
            // PasswordBoxImage
            // 
            this.PasswordBoxImage.Image = ((System.Drawing.Image)(resources.GetObject("PasswordBoxImage.Image")));
            this.PasswordBoxImage.Location = new System.Drawing.Point(33, 385);
            this.PasswordBoxImage.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordBoxImage.Name = "PasswordBoxImage";
            this.PasswordBoxImage.Size = new System.Drawing.Size(475, 69);
            this.PasswordBoxImage.TabIndex = 1;
            this.PasswordBoxImage.TabStop = false;
            // 
            // LoginButton
            // 
            this.LoginButton.Image = ((System.Drawing.Image)(resources.GetObject("LoginButton.Image")));
            this.LoginButton.Location = new System.Drawing.Point(109, 491);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(4);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(349, 59);
            this.LoginButton.TabIndex = 2;
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.AcceptsTab = true;
            this.UsernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTextBox.Location = new System.Drawing.Point(109, 276);
            this.UsernameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(397, 64);
            this.UsernameTextBox.TabIndex = 3;
            this.UsernameTextBox.TextChanged += new System.EventHandler(this.UsernameTextBox_TextChanged);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.AcceptsTab = true;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(109, 385);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(397, 64);
            this.PasswordTextBox.TabIndex = 4;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // LoginLogo
            // 
            this.LoginLogo.BackColor = System.Drawing.Color.Transparent;
            this.LoginLogo.Image = ((System.Drawing.Image)(resources.GetObject("LoginLogo.Image")));
            this.LoginLogo.Location = new System.Drawing.Point(84, 14);
            this.LoginLogo.Margin = new System.Windows.Forms.Padding(4);
            this.LoginLogo.Name = "LoginLogo";
            this.LoginLogo.Size = new System.Drawing.Size(363, 219);
            this.LoginLogo.TabIndex = 5;
            this.LoginLogo.TabStop = false;
            // 
            // ErrorMessageTextBox
            // 
            this.ErrorMessageTextBox.BackColor = System.Drawing.Color.White;
            this.ErrorMessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ErrorMessageTextBox.ForeColor = System.Drawing.Color.Red;
            this.ErrorMessageTextBox.Location = new System.Drawing.Point(109, 469);
            this.ErrorMessageTextBox.Name = "ErrorMessageTextBox";
            this.ErrorMessageTextBox.ReadOnly = true;
            this.ErrorMessageTextBox.Size = new System.Drawing.Size(349, 15);
            this.ErrorMessageTextBox.TabIndex = 6;
            this.ErrorMessageTextBox.Text = "Error Message";
            this.ErrorMessageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ErrorMessageTextBox.Visible = false;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(568, 570);
            this.Controls.Add(this.ErrorMessageTextBox);
            this.Controls.Add(this.LoginLogo);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordBoxImage);
            this.Controls.Add(this.UsernameBoxImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.UsernameBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox UsernameBoxImage;
        private System.Windows.Forms.PictureBox PasswordBoxImage;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.PictureBox LoginLogo;
        private System.Windows.Forms.TextBox ErrorMessageTextBox;
    }
}

