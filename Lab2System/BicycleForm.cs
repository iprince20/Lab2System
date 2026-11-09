using System;
using System.Windows.Forms;

namespace Lab2System
{
    public partial class BicycleForm : Form
    {
        public BicycleForm()
        {
            InitializeComponent();
            Theme.Apply(this);
        }

        // Rate for the hour starting at 'hour' (0-23).
        private static int RateForHour(int hour)
        {
            if (hour < 7 || hour >= 21) return 500;   // 0-7 and 21-24
            if (hour < 14 || hour >= 19) return 1000; // 7-14 and 19-21
            return 1500;                              // 14-19
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            int start, end;
            if (!int.TryParse(txtStart.Text.Trim(), out start) || !int.TryParse(txtEnd.Text.Trim(), out end))
            {
                Warn("Please enter whole numbers only (decimals are not accepted).");
                return;
            }
            if (start < 0 || start > 23) { Warn("Start time must be between 0 and 23."); return; }
            if (end < 1 || end > 24) { Warn("End time must be between 1 and 24."); return; }
            if (start >= end) { Warn("Start time must be less than end time."); return; }

            int total = 0;
            for (int h = start; h < end; h++)
                total += RateForHour(h);

            lblResult.Text = string.Format("Rental {0}h - {1}h ({2} hour(s))\nTotal to pay: {3} RWF",
                start, end, end - start, total);
        }

        private static void Warn(string msg)
        {
            MessageBox.Show(msg, "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Navigation.Logout(this);
        }
    }
}
