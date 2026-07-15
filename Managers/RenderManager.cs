using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AP_Final_Project.Managers
{
    public class RenderManager
    {
        private Bitmap? backgroundImage;

        public RenderManager(string equippedBackground)
        {
            backgroundImage = CreateBackground(equippedBackground);
        }

        public void Render(Graphics g, int width, int height,
            Player player, List<Enemy> enemies, List<Bullet> bullets,
            List<Coin> coins, List<PowerUp> powerUps,
            int currentWave, bool isPaused, bool isGameOver, int finalScore,
            int highScore, string waveMessage, int announcementTimer)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawBackground(g, width, height);

            foreach (var coin in coins) coin.Draw(g);
            foreach (var powerUp in powerUps) powerUp.Draw(g);
            foreach (var bullet in bullets) bullet.Draw(g);
            foreach (var enemy in enemies) if (enemy.IsAlive) enemy.Draw(g);

            player.Draw(g);

            DrawHUD(g, width, player, currentWave);

            if (announcementTimer > 0 && !string.IsNullOrEmpty(waveMessage))
                DrawWaveAnnouncement(g, width, height, waveMessage, announcementTimer);

            if (isPaused) DrawPauseOverlay(g, width, height);
            if (isGameOver) DrawGameOverOverlay(g, width, height, finalScore, highScore);
        }

        private void DrawBackground(Graphics g, int width, int height)
        {
            if (backgroundImage != null)
            {
                g.DrawImage(backgroundImage, 0, 0, width, height);
            }
            else
            {
                using var brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, 0, 0, width, height);
            }
        }

        private void DrawHUD(Graphics g, int canvasWidth, Player player, int currentWave)
        {
            int barWidth = 150;
            int barHeight = 16;
            float barX = 10;
            float barY = 10;

            using var bgBrush = new SolidBrush(Color.DarkGray);
            g.FillRectangle(bgBrush, barX, barY, barWidth, barHeight);

            float hpRatio = (float)player.HP / player.MaxHP;
            Color hpColor = hpRatio > 0.5f ? Color.LimeGreen : (hpRatio > 0.25f ? Color.Yellow : Color.Red);
            using var hpBrush = new SolidBrush(hpColor);
            g.FillRectangle(hpBrush, barX, barY, barWidth * hpRatio, barHeight);

            using var borderPen = new Pen(Color.White, 1);
            g.DrawRectangle(borderPen, barX, barY, barWidth, barHeight);

            using var hudFont = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString($"HP: {player.HP}/{player.MaxHP}", hudFont, Brushes.White, barX, barY + barHeight + 2);

            float livesX = barX;
            float livesY = barY + barHeight + 18;
            g.DrawString($"Lives: {player.Lives}", hudFont, Brushes.White, livesX, livesY);

            string scoreText = $"Score: {player.Score}";
            var scoreSize = g.MeasureString(scoreText, hudFont);
            g.DrawString(scoreText, hudFont, Brushes.Yellow, canvasWidth - scoreSize.Width - 10, 10);

            
            string coinText = $"Coins: {player.CoinsCollected}";
            var coinSize = g.MeasureString(coinText, hudFont);
            g.DrawString(coinText, hudFont, Brushes.Gold, canvasWidth - coinSize.Width - 10, 28);

            
            string waveText = $"Wave: {currentWave}/{GameConfig.TotalWaves}";
            var waveSize = g.MeasureString(waveText, hudFont);
            g.DrawString(waveText, hudFont, Brushes.Cyan, canvasWidth - waveSize.Width - 10, 46);

            float puY = livesY + 20;
            if (player.TripleShotTimer > 0)
            {
                DrawPowerUpTimer(g, barX, puY, "Triple Shot", player.TripleShotTimer, GameConfig.TripleShotDurationMs, Color.Orange);
                puY += 18;
            }
            if (player.FireRateTimer > 0)
            {
                DrawPowerUpTimer(g, barX, puY, "Fire Rate", player.FireRateTimer, GameConfig.FireRateDurationMs, Color.Yellow);
            }
        }

        private void DrawPowerUpTimer(Graphics g, float x, float y, string name, int remaining, int total, Color color)
        {
            int barWidth = 100;
            int barHeight = 10;
            float ratio = (float)remaining / total;

            using var bgBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
            g.FillRectangle(bgBrush, x, y, barWidth + 60, barHeight);

            using var fillBrush = new SolidBrush(color);
            g.FillRectangle(fillBrush, x, y, barWidth * ratio, barHeight);

            using var font = new Font("Segoe UI", 7, FontStyle.Bold);
            g.DrawString($"{name} {(remaining / 1000f):F1}s", font, Brushes.White, x + barWidth + 4, y - 1);
        }

        private void DrawWaveAnnouncement(Graphics g, int width, int height, string message, int timer)
        {
            float alpha = Math.Min(1f, timer / 500f);
            int alphaInt = (int)(alpha * 255);

            using var font = new Font("Segoe UI", 36, FontStyle.Bold);
            var size = g.MeasureString(message, font);
            float x = (width - size.Width) / 2;
            float y = (height - size.Height) / 2;

            using var textBrush = new SolidBrush(Color.FromArgb(alphaInt, Color.White));
            g.DrawString(message, font, textBrush, x, y);
        }

        private void DrawPauseOverlay(Graphics g, int width, int height)
        {
            using var overlayBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
            g.FillRectangle(overlayBrush, 0, 0, width, height);

            using var font = new Font("Segoe UI", 48, FontStyle.Bold);
            var size = g.MeasureString("PAUSED", font);
            g.DrawString("PAUSED", font, Brushes.White,
                (width - size.Width) / 2, (height - size.Height) / 2);

            using var smallFont = new Font("Segoe UI", 14);
            var smallSize = g.MeasureString("Press ESC to resume", smallFont);
            g.DrawString("Press ESC to resume", smallFont, Brushes.LightGray,
                (width - smallSize.Width) / 2, (height - size.Height) / 2 + 60);
        }

        private void DrawGameOverOverlay(Graphics g, int width, int height, int finalScore, int highScore)
        {
            using var overlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
            g.FillRectangle(overlayBrush, 0, 0, width, height);

            using var titleFont = new Font("Segoe UI", 42, FontStyle.Bold);
            string title = finalScore >= highScore ? "VICTORY!" : "GAME OVER";
            var titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Gold,
                (width - titleSize.Width) / 2, height / 2 - 80);

            using var scoreFont = new Font("Segoe UI", 20, FontStyle.Bold);
            string scoreText = $"Final Score: {finalScore}";
            var scoreSize = g.MeasureString(scoreText, scoreFont);
            g.DrawString(scoreText, scoreFont, Brushes.White,
                (width - scoreSize.Width) / 2, height / 2 - 10);

            string highText = $"High Score: {highScore}";
            var highSize = g.MeasureString(highText, scoreFont);
            g.DrawString(highText, scoreFont, Brushes.Gold,
                (width - highSize.Width) / 2, height / 2 + 30);

            using var smallFont = new Font("Segoe UI", 14);
            var smallSize = g.MeasureString("Press any key to return to menu", smallFont);
            g.DrawString("Press any key to return to menu", smallFont, Brushes.LightGray,
                (width - smallSize.Width) / 2, height / 2 + 80);
        }

        private Bitmap? CreateBackground(string equippedBackground)
        {
            int w = GameConfig.CanvasWidth;
            int h = GameConfig.CanvasHeight;

            if (equippedBackground.Contains("Galaxy"))
                return CreateGalaxyBackground(w, h);
            if (equippedBackground.Contains("Mars"))
                return CreateMarsBackground(w, h);
            if (equippedBackground.Contains("Neon"))
                return CreateNeonCityBackground(w, h);

            return CreateDefaultBackground(w, h);
        }

        private Bitmap CreateDefaultBackground(int w, int h)
        {
            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, h),
                Color.FromArgb(5, 5, 20), Color.FromArgb(15, 15, 40));
            g.FillRectangle(brush, 0, 0, w, h);
            DrawStars(g, w, h, 80);
            return bmp;
        }

        private Bitmap CreateGalaxyBackground(int w, int h)
        {
            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(w, h),
                Color.Navy, Color.FromArgb(10, 0, 40, 80));
            g.FillRectangle(brush, 0, 0, w, h);
            DrawStars(g, w, h, 150);
            return bmp;
        }

        private Bitmap CreateMarsBackground(int w, int h)
        {
            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, h),
                Color.FromArgb(40, 10, 5), Color.FromArgb(80, 20, 10));
            g.FillRectangle(brush, 0, 0, w, h);
            DrawStars(g, w, h, 40);
            return bmp;
        }

        private Bitmap CreateNeonCityBackground(int w, int h)
        {
            var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, h),
                Color.FromArgb(20, 0, 20), Color.FromArgb(40, 0, 40));
            g.FillRectangle(brush, 0, 0, w, h);

            var rng = new Random(42);
            using var linePen1 = new Pen(Color.FromArgb(100, 255, 0, 255), 1);
            using var linePen2 = new Pen(Color.FromArgb(100, 0, 255, 255), 1);
            for (int i = 0; i < 15; i++)
            {
                var pen = rng.Next(2) == 0 ? linePen1 : linePen2;
                int y = rng.Next(h);
                g.DrawLine(pen, 0, y, w, y);
            }
            DrawStars(g, w, h, 30);
            return bmp;
        }

        private void DrawStars(Graphics g, int w, int h, int count)
        {
            var rng = new Random(123);
            using var starBrush = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
            for (int i = 0; i < count; i++)
            {
                int x = rng.Next(w);
                int y = rng.Next(h);
                int size = rng.Next(1, 3);
                g.FillEllipse(starBrush, x, y, size, size);
            }
        }
    }
}
