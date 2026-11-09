using System.Linq;
using System.Windows.Forms;

namespace Lab2System
{
    internal static class Navigation
    {
        // Close every service/choose form and return to the (hidden) login form.
        public static void Logout(Form current)
        {
            Form1 login = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (login != null)
            {
                login.ResetFields();
                login.Show();
            }
            foreach (Form f in Application.OpenForms.Cast<Form>().Where(f => !(f is Form1)).ToList())
            {
                f.Tag = "logout";
                f.Close();
            }
        }
    }
}
