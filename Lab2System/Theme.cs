using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Lab2System
{
    // Modern look for every form: gradient background, white rounded "card" with shadow,
    // rounded buttons with hover, rounded input boxes. Call Theme.Apply(this) after InitializeComponent().
    internal static class Theme
    {
        public static readonly Color BgTop = Color.FromArgb(224, 231, 255);
        public static readonly Color BgBottom = Color.FromArgb(248, 250, 252);
        public static readonly Color Card = Color.White;
        public static readonly Color Header = Color.FromArgb(30, 41, 59);
        public static readonly Color Text = Color.FromArgb(15, 23, 42);
        public static readonly Color Muted = Color.FromArgb(100, 116, 139);
        public static readonly Color Border = Color.FromArgb(203, 213, 225);
        public static readonly Color Primary = Color.FromArgb(79, 70, 229);
        public static readonly Color Success = Color.FromArgb(16, 185, 129);
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);
        public static readonly Color Secondary = Color.FromArgb(100, 116, 139);

        private const int Radius = 12;
        private static readonly HashSet<Button> hovered = new HashSet<Button>();

        public static void Apply(Form form)
        {
            form.BackColor = BgBottom;
            form.ForeColor = Text;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(form, true, null);

            StyleControls(form, form.Controls);
            form.Paint += PaintForm;
        }

        private static void StyleControls(Form form, Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is Button) StyleButton((Button)c);
                else if (c is TextBox)
                {
                    TextBox t = (TextBox)c;
                    t.BorderStyle = BorderStyle.None;
                    t.Font = new Font("Segoe UI", 12F);
                    t.BackColor = Card;
                    t.ForeColor = Text;
                    t.Enter += (s, e) => form.Invalidate();
                    t.Leave += (s, e) => form.Invalidate();
                }
                else if (c is Panel)
                {
                    c.BackColor = Header;
                    foreach (Control inner in c.Controls)
                    {
                        if (inner is Label) { inner.BackColor = Header; inner.ForeColor = Color.White; inner.Font = new Font("Segoe UI", 16F, FontStyle.Bold); }
                        else if (inner is Button) StyleButton((Button)inner);
                    }
                    continue;
                }
                else if (c is Label)
                {
                    Label l = (Label)c;
                    l.BackColor = Color.Transparent;
                    l.Font = new Font("Segoe UI", l.Font.Size, l.Font.Style);
                    l.ForeColor = Text;
                    if (l.Name == "lblTitle") { l.Font = new Font("Segoe UI", 18F, FontStyle.Bold); l.ForeColor = Header; }
                    else if (l.Name == "lblResult") l.ForeColor = Success;
                    else if (l.Name == "lblRates" || l.Name == "lblInfo") l.ForeColor = Muted;
                }
                if (c.HasChildren && !(c is TextBox)) StyleControls(form, c.Controls);
            }
        }

        // ---------- Form background, card and input borders ----------

        private static void PaintForm(object sender, PaintEventArgs e)
        {
            Form form = (Form)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bg = new LinearGradientBrush(form.ClientRectangle, BgTop, BgBottom, 90f))
                g.FillRectangle(bg, form.ClientRectangle);

            Rectangle card = CardBounds(form);
            if (card != Rectangle.Empty)
            {
                for (int i = 6; i >= 1; i--) // soft shadow
                {
                    Rectangle s = card; s.Inflate(i, i); s.Offset(0, 4);
                    using (var p = RoundRect(s, Radius + i))
                    using (var b = new SolidBrush(Color.FromArgb(10, 30, 41, 59)))
                        g.FillPath(b, p);
                }
                using (var p = RoundRect(card, Radius * 2))
                using (var b = new SolidBrush(Card))
                    g.FillPath(b, p);
            }

            foreach (TextBox t in form.Controls.OfType<TextBox>())
            {
                Rectangle r = t.Bounds; r.Inflate(10, 8);
                using (var p = RoundRect(r, 8))
                using (var fill = new SolidBrush(Card))
                using (var pen = new Pen(t.Focused ? Primary : Border, t.Focused ? 2f : 1.2f))
                {
                    g.FillPath(fill, p);
                    g.DrawPath(pen, p);
                }
            }
        }

        // White card behind every control except the header panel.
        private static Rectangle CardBounds(Form form)
        {
            Rectangle r = Rectangle.Empty;
            foreach (Control c in form.Controls)
            {
                if (c is Panel || !c.Visible) continue;
                r = r == Rectangle.Empty ? c.Bounds : Rectangle.Union(r, c.Bounds);
            }
            if (r == Rectangle.Empty) return r;
            r.Inflate(30, 25);
            return Rectangle.Intersect(r, Rectangle.Inflate(form.ClientRectangle, -12, -12));
        }

        // ---------- Rounded buttons ----------

        private static Color ButtonColor(Button b)
        {
            string key = (b.Name + " " + b.Text).ToUpper();
            if (key.Contains("LOGOUT")) return Danger;
            if (key.Contains("MUSHROOM") || key.Contains("BTNYES")) return Success;
            if (key.Contains("BTNNO") || key.Contains("RESTART") || key.Contains("SIGN") || key.Contains("HOME")) return Secondary;
            return Primary;
        }

        private static void StyleButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;
            b.Font = new Font("Segoe UI", b.Height > 80 ? 14F : 10.5F, FontStyle.Bold);
            b.MouseEnter += (s, e) => { hovered.Add(b); b.Invalidate(); };
            b.MouseLeave += (s, e) => { hovered.Remove(b); b.Invalidate(); };
            b.EnabledChanged += (s, e) => b.Invalidate();
            b.Paint += PaintButton;
        }

        private static void PaintButton(object sender, PaintEventArgs e)
        {
            Button b = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Corners take the colour of whatever is behind the button (header panel or white card).
            g.Clear(b.Parent is Panel ? b.Parent.BackColor : Card);

            Color color = ButtonColor(b);
            if (!b.Enabled) color = Color.FromArgb(203, 213, 225);
            else if (hovered.Contains(b)) color = ControlPaint.Dark(color, 0.08f);

            Rectangle r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
            int radius = b.Height > 80 ? 20 : Radius;
            using (var p = RoundRect(r, radius))
            using (var brush = new LinearGradientBrush(r, ControlPaint.Light(color, 0.15f), color, 90f))
                g.FillPath(brush, p);

            TextRenderer.DrawText(g, b.Text, b.Font, r, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
