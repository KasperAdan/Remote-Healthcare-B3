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
        public string username { get; set; }
        public string password { get; set; }

        public string error { get; set; }
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
    }
}
