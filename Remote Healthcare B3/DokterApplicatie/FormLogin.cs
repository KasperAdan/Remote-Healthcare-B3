using System;
using System.Windows.Forms;

namespace DokterApplicatie
{
    public partial class FormLogin : Form
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Error { get; set; }
        public FormLogin()
        {
            InitializeComponent();
        }

        public FormLogin(string error)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ErrorMessageTextBox.Visible = true;
            ErrorMessageTextBox.Text = error;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (Username == "" || Username == null)
            {
                ErrorMessageTextBox.Text = "Enter username";
                ErrorMessageTextBox.Visible = true;
                return;
            }
            if (Password == "" || Password == null)
            {
                ErrorMessageTextBox.Text = "Enter password";
                ErrorMessageTextBox.Visible = true;
                return;
            }
            DialogResult = DialogResult.Yes;
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Username = UsernameTextBox.Text;
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Password = PasswordTextBox.Text;
        }
    }
}
