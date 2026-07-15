using System;
using System.Drawing;
using System.Windows.Forms;

namespace AP_Final_Project.UI
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            this.Text = "ABOUT";
            this.BackColor = Color.FromArgb(20, 20, 35);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(450, 350);
            this.Size = new Size(500, 400);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(30)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            Label lblInfo = new Label
            {
                Text = "SPACE SHOOTER\n\n" +
                       "Version 1.0\n\n" +
                       "Developed by:\n[Seyed Mohammad Mahdi Farzaneh] & [Seyed Mohammad Taha Mousavi]\n\n" +
                       "Student ID: [403413221] & [403414054]\n\n" ,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            Button btnOk = new Button
            {
                Text = "OK",
                Dock = DockStyle.Fill,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnOk.Click += (s, e) => this.Close();

            layout.Controls.Add(lblInfo, 0, 0);
            layout.Controls.Add(btnOk, 0, 1);
            this.Controls.Add(layout);
        }
    }
}
