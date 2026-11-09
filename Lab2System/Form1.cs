using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Theme.Apply(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }

        public void ResetFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Get username and password
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Validate empty username
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter your username.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtUsername.Focus();
                return;
            }

            // Validate empty password
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your password.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPassword.Focus();
                return;
            }

            // Check login credentials
            if (username == "admin" && password == "1234")
            {
                MessageBox.Show("Login successful!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ChooseForm chooseForm = new ChooseForm();
                chooseForm.FormClosed += (s2, e2) => { if (!this.Visible) Application.Exit(); };

                chooseForm.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect username or password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

    }
}
