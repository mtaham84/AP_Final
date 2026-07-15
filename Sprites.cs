using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AP_Final_Project
{
    public enum PowerUpColor { Orange, Cyan, Green, Yellow }

    public static class SpriteGenerator
    {
        public static Bitmap CreatePlayerDefault()
        {
            var bmp = new Bitmap(40, 48);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[]
            {
                new PointF(20, 0), new PointF(5, 44), new PointF(17, 38),
                new PointF(20, 48), new PointF(23, 38), new PointF(35, 44)
            };
            using var brush = new SolidBrush(Color.FromArgb(200, 200, 210));
            using var pen = new Pen(Color.White, 1);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            return bmp;
        }

        public static Bitmap CreatePlayerRedEagle()
        {
            var bmp = new Bitmap(40, 48);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[]
            {
                new PointF(20, 0), new PointF(3, 44), new PointF(14, 36),
                new PointF(20, 48), new PointF(26, 36), new PointF(37, 44)
            };
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(40, 48), Color.Red, Color.DarkRed);
            using var pen = new Pen(Color.OrangeRed, 1);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            using var wingBrush = new SolidBrush(Color.OrangeRed);
            g.FillRectangle(wingBrush, 0, 30, 8, 4);
            g.FillRectangle(wingBrush, 32, 30, 8, 4);
            return bmp;
        }

        public static Bitmap CreatePlayerCyberPhantom()
        {
            var bmp = new Bitmap(40, 48);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[]
            {
                new PointF(20, 0), new PointF(2, 42), new PointF(15, 34),
                new PointF(20, 48), new PointF(25, 34), new PointF(38, 42)
            };
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(40, 48), Color.MediumPurple, Color.DarkViolet);
            using var pen = new Pen(Color.Cyan, 1);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            using var glow = new SolidBrush(Color.FromArgb(80, 0, 255, 255));
            g.FillEllipse(glow, 8, 16, 24, 16);
            return bmp;
        }

        public static Bitmap CreateEnemyStandard()
        {
            var bmp = new Bitmap(36, 36);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[] { new(18, 0), new(0, 18), new(18, 36), new(36, 18) };
            using var brush = new SolidBrush(Color.FromArgb(220, 40, 40));
            using var pen = new Pen(Color.Red, 1.5f);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            return bmp;
        }

        public static Bitmap CreateEnemyScout()
        {
            var bmp = new Bitmap(30, 30);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[] { new(15, 0), new(0, 30), new(30, 30) };
            using var brush = new SolidBrush(Color.FromArgb(230, 140, 20));
            using var pen = new Pen(Color.Orange, 1.5f);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            return bmp;
        }

        public static Bitmap CreateEnemyShooter()
        {
            var bmp = new Bitmap(38, 38);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var points = new PointF[]
            {
                new(19, 0), new(36, 9), new(36, 29),
                new(19, 38), new(2, 29), new(2, 9)
            };
            using var brush = new SolidBrush(Color.FromArgb(200, 180, 20));
            using var pen = new Pen(Color.Gold, 1.5f);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            using var innerBrush = new SolidBrush(Color.DarkGoldenrod);
            g.FillEllipse(innerBrush, 12, 12, 14, 14);
            return bmp;
        }

        public static Bitmap CreateEnemyHeavyTank()
        {
            var bmp = new Bitmap(50, 50);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(Color.FromArgb(140, 30, 30));
            using var pen = new Pen(Color.DarkRed, 2);
            g.FillRectangle(brush, 4, 4, 42, 42);
            g.DrawRectangle(pen, 4, 4, 42, 42);
            using var innerBrush = new SolidBrush(Color.FromArgb(180, 60, 60));
            g.FillRectangle(innerBrush, 10, 10, 30, 30);
            using var rivetBrush = new SolidBrush(Color.Gray);
            g.FillEllipse(rivetBrush, 6, 6, 6, 6);
            g.FillEllipse(rivetBrush, 38, 6, 6, 6);
            g.FillEllipse(rivetBrush, 6, 38, 6, 6);
            g.FillEllipse(rivetBrush, 38, 38, 6, 6);
            return bmp;
        }

        public static Bitmap CreateGreenLaser()
        {
            var bmp = new Bitmap(6, 14);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, 14), Color.LimeGreen, Color.DarkGreen);
            g.FillRectangle(brush, 1, 0, 4, 14);
            return bmp;
        }

        public static Bitmap CreatePlasmaBullet()
        {
            var bmp = new Bitmap(8, 16);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, 16), Color.Cyan, Color.DarkBlue);
            g.FillEllipse(brush, 0, 0, 8, 16);
            return bmp;
        }

        public static Bitmap CreateEnemyBullet()
        {
            var bmp = new Bitmap(6, 10);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, 10), Color.OrangeRed, Color.DarkRed);
            g.FillRectangle(brush, 1, 0, 4, 10);
            return bmp;
        }

        public static Bitmap CreateSilverCoin()
        {
            var bmp = new Bitmap(18, 18);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(18, 18), Color.LightGray, Color.Gray);
            using var pen = new Pen(Color.DarkGray, 1);
            g.FillEllipse(brush, 1, 1, 16, 16);
            g.DrawEllipse(pen, 1, 1, 16, 16);
            using var font = new Font("Arial", 8, FontStyle.Bold);
            g.DrawString("S", font, Brushes.Black, 4, 3);
            return bmp;
        }

        public static Bitmap CreateGoldCoin()
        {
            var bmp = new Bitmap(22, 22);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new LinearGradientBrush(new Point(0, 0), new Point(22, 22), Color.Gold, Color.DarkGoldenrod);
            using var pen = new Pen(Color.Brown, 1);
            g.FillEllipse(brush, 1, 1, 20, 20);
            g.DrawEllipse(pen, 1, 1, 20, 20);
            using var font = new Font("Arial", 9, FontStyle.Bold);
            g.DrawString("G", font, Brushes.Black, 5, 3);
            return bmp;
        }

        public static Bitmap CreatePowerUp(PowerUpColor color, string letter)
        {
            var bmp = new Bitmap(28, 28);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            Color bgColor = color switch
            {
                PowerUpColor.Orange => Color.FromArgb(220, 140, 30),
                PowerUpColor.Cyan => Color.FromArgb(30, 180, 200),
                PowerUpColor.Green => Color.FromArgb(30, 180, 60),
                PowerUpColor.Yellow => Color.FromArgb(220, 200, 30),
                _ => Color.White
            };
            using var brush = new SolidBrush(bgColor);
            using var pen = new Pen(Color.White, 2);
            g.FillRectangle(brush, 2, 2, 24, 24);
            g.DrawRectangle(pen, 2, 2, 24, 24);
            using var font = new Font("Arial", 14, FontStyle.Bold);
            var textSize = g.MeasureString(letter, font);
            g.DrawString(letter, font, Brushes.Black, (28 - textSize.Width) / 2, (28 - textSize.Height) / 2);
            return bmp;
        }
    }

    public static class Sprites
    {
        private static Bitmap? playerDefault, playerRedEagle, playerCyberPhantom;
        private static Bitmap? enemyStandard, enemyScout, enemyShooter, enemyHeavyTank;
        private static Bitmap? bulletGreenLaser, bulletPlasma, bulletEnemy;
        private static Bitmap? coinSilver, coinGold;
        private static Bitmap? powerUpTripleShot, powerUpHealth, powerUpFireRate;

        public static Bitmap PlayerDefault => playerDefault ??= SpriteGenerator.CreatePlayerDefault();
        public static Bitmap PlayerRedEagle => playerRedEagle ??= SpriteGenerator.CreatePlayerRedEagle();
        public static Bitmap PlayerCyberPhantom => playerCyberPhantom ??= SpriteGenerator.CreatePlayerCyberPhantom();
        public static Bitmap EnemyStandard => enemyStandard ??= SpriteGenerator.CreateEnemyStandard();
        public static Bitmap EnemyScout => enemyScout ??= SpriteGenerator.CreateEnemyScout();
        public static Bitmap EnemyShooter => enemyShooter ??= SpriteGenerator.CreateEnemyShooter();
        public static Bitmap EnemyHeavyTank => enemyHeavyTank ??= SpriteGenerator.CreateEnemyHeavyTank();
        public static Bitmap BulletGreenLaser => bulletGreenLaser ??= SpriteGenerator.CreateGreenLaser();
        public static Bitmap BulletPlasma => bulletPlasma ??= SpriteGenerator.CreatePlasmaBullet();
        public static Bitmap BulletEnemy => bulletEnemy ??= SpriteGenerator.CreateEnemyBullet();
        public static Bitmap CoinSilver => coinSilver ??= SpriteGenerator.CreateSilverCoin();
        public static Bitmap CoinGold => coinGold ??= SpriteGenerator.CreateGoldCoin();
        public static Bitmap PowerUpTripleShot => powerUpTripleShot ??= SpriteGenerator.CreatePowerUp(PowerUpColor.Orange, "T");
        public static Bitmap PowerUpHealth => powerUpHealth ??= SpriteGenerator.CreatePowerUp(PowerUpColor.Green, "+");
        public static Bitmap PowerUpFireRate => powerUpFireRate ??= SpriteGenerator.CreatePowerUp(PowerUpColor.Yellow, "F");
    }
}
