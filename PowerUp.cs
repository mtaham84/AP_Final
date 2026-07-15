using System.Drawing;

namespace AP_Final_Project
{
    public enum PowerUpType { TripleShot, HealthPack, FireRate }

    public abstract class PowerUp : GameObject
    {
        public PowerUpType Type { get; }
        public int DurationMs { get; }

        protected PowerUp(float x, float y, PowerUpType type, int durationMs)
            : base(x, y, GameConfig.PowerUpSize, GameConfig.PowerUpSize,
                  GameConfig.PowerUpFallSpeed, 1)
        {
            Type = type;
            DurationMs = durationMs;
        }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            if (Sprite != null)
                g.DrawImage(Sprite, X, Y, Width, Height);
            else
                DrawPlaceholder(g);
        }

        protected virtual void DrawPlaceholder(Graphics g)
        {
            Color bgColor = Type switch
            {
                PowerUpType.TripleShot => Color.Orange,
                PowerUpType.HealthPack => Color.LimeGreen,
                PowerUpType.FireRate => Color.Yellow,
                _ => Color.White
            };
            string label = Type switch
            {
                PowerUpType.TripleShot => "T",
                PowerUpType.HealthPack => "+",
                PowerUpType.FireRate => "F",
                _ => "?"
            };
            using var brush = new SolidBrush(bgColor);
            using var borderPen = new Pen(Color.White, 2);
            g.FillRectangle(brush, X, Y, Width, Height);
            g.DrawRectangle(borderPen, X, Y, Width, Height);
            using var font = new Font("Arial", 12, FontStyle.Bold);
            var textSize = g.MeasureString(label, font);
            g.DrawString(label, font, Brushes.Black,
                X + (Width - textSize.Width) / 2,
                Y + (Height - textSize.Height) / 2);
        }
    }

    public class TripleShotPowerUp : PowerUp
    {
        public TripleShotPowerUp(float x, float y)
            : base(x, y, PowerUpType.TripleShot, GameConfig.TripleShotDurationMs) { }
    }

    public class HealthPackPowerUp : PowerUp
    {
        public HealthPackPowerUp(float x, float y)
            : base(x, y, PowerUpType.HealthPack, 0) { }
    }

    public class FireRatePowerUp : PowerUp
    {
        public FireRatePowerUp(float x, float y)
            : base(x, y, PowerUpType.FireRate, GameConfig.FireRateDurationMs) { }
    }
}
