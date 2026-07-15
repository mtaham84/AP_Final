using System;
using System.Drawing;
using System.Windows.Forms;

namespace AP_Final_Project.UI
{
    public class MainMenuForm : Form
    {
        private TableLayoutPanel mainLayout = null!;
        private Label lblTitle = null!;
        private Button btnPlay = null!;
        private Button btnShop = null!;
        private Button btnOptions = null!;
        private Button btnAbout = null!;
        private Button btnQuit = null!;
        private Label lblCoins = null!;
        private Label lblHighScore = null!;

        public MainMenuForm()
        {
            DatabaseHelper.InitializeDatabase();
            InitializeComponents();
            UpdateStatsDisplay();
            this.Resize += (s, e) => AdjustFonts();
        }

        private void InitializeComponents()
        {
            this.Text = "Space Shooter";
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 500);
            this.Size = new Size(800, 600);

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(40)
            };
            mainLayout.RowStyles.Clear();
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            lblTitle = new Label
            {
                Text = "SPACE SHOOTER",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 230, 230),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            buttonPanel.Padding = new Padding(10);

            btnPlay = CreateStylishButton("PLAY", Color.FromArgb(0, 120, 200));
            btnShop = CreateStylishButton("SHOP", Color.FromArgb(0, 150, 100));
            btnOptions = CreateStylishButton("OPTIONS", Color.FromArgb(200, 120, 0));
            btnAbout = CreateStylishButton("ABOUT", Color.FromArgb(100, 100, 200));
            btnQuit = CreateStylishButton("QUIT", Color.FromArgb(180, 0, 0));

            buttonPanel.Controls.Add(btnPlay);
            buttonPanel.Controls.Add(btnShop);
            buttonPanel.Controls.Add(btnOptions);
            buttonPanel.Controls.Add(btnAbout);
            buttonPanel.Controls.Add(btnQuit);

            TableLayoutPanel infoPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            lblCoins = new Label
            {
                Text = "Coins: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            lblHighScore = new Label
            {
                Text = "High Score: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Orange,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            infoPanel.Controls.Add(lblCoins, 0, 0);
            infoPanel.Controls.Add(lblHighScore, 1, 0);

            btnPlay.Click += (s, e) =>
            {
                var skinItem = DatabaseHelper.GetEquippedItemByCategory("Ship Skin");
                var bulletItem = DatabaseHelper.GetEquippedItemByCategory("Bullet Style");
                var bgItem = DatabaseHelper.GetEquippedItemByCategory("Background Theme");

                string skin = skinItem?.Name ?? "Ship Skin - Default";
                string bullet = bulletItem?.Name ?? "Bullet Style - Green Laser";
                string bg = bgItem?.Name ?? "Default";

                new GameForm(skin, bullet, bg).ShowDialog();
                UpdateStatsDisplay();
            };
            btnShop.Click += (s, e) => { new ShopForm().ShowDialog(); UpdateStatsDisplay(); };
            btnOptions.Click += (s, e) => new OptionsForm().ShowDialog();
            btnAbout.Click += (s, e) => new AboutForm().ShowDialog();
            btnQuit.Click += (s, e) => Application.Exit();

            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(buttonPanel, 0, 1);
            mainLayout.Controls.Add(infoPanel, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private Button CreateStylishButton(string text, Color backColor, int fontSize = 14)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 55,
                Width = 250,
                Margin = new Padding(10, 5, 10, 5)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.3f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.2f);
            return btn;
        }

        private void AdjustFonts()
        {
            int newSize = Math.Max(18, this.Width / 25);
            lblTitle.Font = new Font("Segoe UI", newSize, FontStyle.Bold);
        }

        private void UpdateStatsDisplay()
        {
            lblCoins.Text = $"Coins: {DatabaseHelper.GetCoins()}";
            lblHighScore.Text = $"High Score: {DatabaseHelper.GetHighScore()}";
        }
    }
}
