using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DokterApplicatie
{
    public partial class FormLogin : Form
    {
        private string Username;
        private string Password;

        public string username
        {
            get
            {
                if (Username == "")
                {
                    return " ";
                }
                return Username;
            }
            set { Username = value; }
        }

        public string password
        {
            get
            {
                if (Password == "")
                {
                    return " ";
                }
                return Password;
            }
            set { Password = value; }
        }

        public string error { get; set; }
        public FormLogin()
        {
            InitializeComponent();
            username = "";
            password = "";
        }

        public FormLogin(string error, string username)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ErrorMessageTextBox.Visible = true;
            ErrorMessageTextBox.Text = error;
            PasswordTextBox.Text = "";
            UsernameTextBox.Text = username;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.username = UsernameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.password = PasswordTextBox.Text;
        }

        private void UsernameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                LoginButton_Click(this, null);
            }
        }

        private void PasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                LoginButton_Click(this, null);
            }
        }
    }
}
