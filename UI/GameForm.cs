using System;
using System.Drawing;
using System.Windows.Forms;
using AP_Final_Project.Managers;

namespace AP_Final_Project.UI
{
    public class GameForm : Form
    {
        private Timer gameTimer = null!;
        private GameManager gameManager = null!;

        public GameForm(string equippedSkin, string equippedBullet, string equippedBackground)
        {
            this.Text = "Game - Space Shooter";
            this.Size = new Size(GameConfig.CanvasWidth, GameConfig.CanvasHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            gameManager = new GameManager(this, equippedSkin, equippedBullet, equippedBackground);

            SetupGameLoop();
            SetupInput();

            this.FormClosed += (s, e) =>
            {
                gameTimer?.Stop();
                gameTimer?.Dispose();
            };
        }

        private void SetupGameLoop()
        {
            gameTimer = new Timer();
            gameTimer.Interval = GameConfig.TimerIntervalMs;
            gameTimer.Tick += (s, e) => this.Invalidate();
            gameTimer.Start();
        }

        private void SetupInput()
        {
            this.KeyDown += (s, e) =>
            {
                gameManager.OnKeyDown(e.KeyCode);

                if (gameManager.IsGameOver)
                {
                    this.Close();
                }
            };
            this.KeyUp += (s, e) => gameManager.OnKeyUp(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            gameManager.Tick(e.Graphics, ClientSize.Width, ClientSize.Height);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            gameTimer?.Stop();
            gameTimer?.Dispose();
        }
    }
}
