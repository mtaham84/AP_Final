using System;
using System.Drawing;
using System.Windows.Forms;

namespace AP_Final_Project.UI
{
    public class OptionsForm : Form
    {
        private TableLayoutPanel mainLayout = null!;
        private Button btnBack = null!;
        private TableLayoutPanel controlsTable = null!;

        public OptionsForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "OPTIONS";
            this.BackColor = Color.FromArgb(20, 20, 35);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(500, 400);
            this.Size = new Size(600, 500);

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(20)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            controlsTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.FromArgb(40, 40, 55),
                ForeColor = Color.White
            };
            controlsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            controlsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            for (int i = 0; i < 6; i++)
                controlsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 6));

            AddControlRow("W / Up", "Move Up");
            AddControlRow("S / Down", "Move Down");
            AddControlRow("A / Left", "Move Left");
            AddControlRow("D / Right", "Move Right");
            AddControlRow("Space", "Shoot");
            AddControlRow("Escape", "Pause");

            btnBack = new Button
            {
                Text = "BACK",
                Dock = DockStyle.Fill,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBack.Click += (s, e) => this.Close();

            mainLayout.Controls.Add(controlsTable, 0, 0);
            mainLayout.Controls.Add(btnBack, 0, 1);
            this.Controls.Add(mainLayout);
        }

        private void AddControlRow(string key, string action)
        {
            Label lblKey = new Label
            {
                Text = key,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Courier New", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.Black
            };
            Label lblAction = new Label
            {
                Text = action,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.Black
            };
            controlsTable.Controls.Add(lblKey);
            controlsTable.Controls.Add(lblAction);
        }
    }
}
