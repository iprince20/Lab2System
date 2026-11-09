using System;
using System.Windows.Forms;

namespace Lab2System
{
    public partial class MushroomForm : Form
    {
        // Current question key; answers walk a decision tree of at most 3 questions.
        private string current;

        public MushroomForm()
        {
            InitializeComponent();
            Theme.Apply(this);
            Restart();
        }

        private void Restart()
        {
            Ask("forest"); // First question must be: does it grow in a forest?
            lblResult.Text = "";
            btnYes.Enabled = btnNo.Enabled = true;
        }

        private void Ask(string key)
        {
            current = key;
            switch (key)
            {
                case "forest": lblQuestion.Text = "Does your mushroom grow in a forest?"; break;
                case "convex":
                case "convex_meadow": lblQuestion.Text = "Does your mushroom have a convex cup?"; break;
                case "ring": lblQuestion.Text = "Does your mushroom have a ring?"; break;
                case "gills": lblQuestion.Text = "Does your mushroom have gills?"; break;
            }
        }

        private void Answer(bool yes)
        {
            switch (current)
            {
                // Forest: Amanite, Cepe, Girolle, Pied bleu. Meadow: Agaric, Coprin.
                case "forest": Ask(yes ? "convex" : "convex_meadow"); break;
                case "convex_meadow": Result(yes ? "Agaric Jaunissant" : "Coprin Chevelu"); break;
                case "convex": Ask(yes ? "ring" : "gills"); break;
                case "ring": Result(yes ? "Amanite Tue-mouche" : "Pied Bleu"); break;
                case "gills": Result(yes ? "Girolle" : "Cepe de Bordeaux"); break;
            }
        }

        private void Result(string name)
        {
            lblQuestion.Text = "Identified!";
            lblResult.Text = "Your mushroom is: " + name;
            btnYes.Enabled = btnNo.Enabled = false;
        }

        private void btnYes_Click(object sender, EventArgs e) { Answer(true); }
        private void btnNo_Click(object sender, EventArgs e) { Answer(false); }
        private void btnRestart_Click(object sender, EventArgs e) { Restart(); }
        private void btnLogout_Click(object sender, EventArgs e) { Navigation.Logout(this); }
    }
}
