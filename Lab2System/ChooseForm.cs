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
    public partial class ChooseForm : Form
    {
        public ChooseForm()
        {
            InitializeComponent();
            Theme.Apply(this);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void btnBicycle_Click(object sender, EventArgs e)
        {
            OpenService(new BicycleForm());
        }

        private void btnMushroom_Click(object sender, EventArgs e)
        {
            OpenService(new MushroomForm());
        }

        private void OpenService(Form service)
        {
            // Closing a service form via the window X brings the user back here;
            // its Logout button goes to the login page instead.
            service.FormClosed += (s, e) => { if (!"logout".Equals(service.Tag)) this.Show(); };
            service.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Navigation.Logout(this);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
