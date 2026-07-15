using System;
using System.Collections.Generic;
using System.Drawing;

namespace AP_Final_Project
{
    public abstract class Enemy : GameObject
    {
        public int ScoreValue { get; }
        public int CoinDropChance { get; }

        protected Enemy(float x, float y, int width, int height, float speed, int hp,
                        int scoreValue, int coinDropChance)
            : base(x, y, width, height, speed, hp)
        {
            ScoreValue = scoreValue;
            CoinDropChance = coinDropChance;
        }

        public override void Update(int canvasWidth, int canvasHeight) => Y += Speed;

        public override void Draw(Graphics g)
        {
            if (Sprite != null) g.DrawImage(Sprite, X, Y, Width, Height);
            else DrawPlaceholder(g);
        }

        protected virtual void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.Red);
            g.FillRectangle(brush, X, Y, Width, Height);
        }

        public virtual bool ShouldShoot() => false;
        public virtual Bullet? Shoot() => null;

        public Coin? DropCoin(Random rng)
        {
            if (rng.Next(100) >= CoinDropChance) return null;
            CoinType type = rng.Next(100) < 80 ? CoinType.Silver : CoinType.Gold;
            return new Coin(X + Width / 2f - 9, Y + Height, type);
        }
    }

    public class StandardEnemy : Enemy
    {
        public StandardEnemy(float x, float y, float speedMultiplier, int hpBonus)
            : base(x, y, GameConfig.StandardEnemyWidth, GameConfig.StandardEnemyHeight,
                  GameConfig.StandardEnemySpeed * speedMultiplier,
                  GameConfig.StandardEnemyHP + hpBonus,
                  GameConfig.StandardEnemyScore, GameConfig.StandardEnemyCoinChance)
        { Sprite = Sprites.EnemyStandard; }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.Red);
            var points = new PointF[] { new(X + Width / 2f, Y), new(X, Y + Height), new(X + Width, Y + Height) };
            g.FillPolygon(brush, points);
        }
    }

    public class ScoutEnemy : Enemy
    {
        private const float SineAmplitude = 50f;
        private const float SineFrequency = 0.05f;
        private float sineOffset;
        private float startX;

        public ScoutEnemy(float x, float y, float speedMultiplier, int hpBonus)
            : base(x, y, GameConfig.ScoutEnemyWidth, GameConfig.ScoutEnemyHeight,
                  GameConfig.ScoutEnemySpeed * speedMultiplier,
                  GameConfig.ScoutEnemyHP + hpBonus,
                  GameConfig.ScoutEnemyScore, GameConfig.ScoutEnemyCoinChance)
        { startX = x; sineOffset = 0; Sprite = Sprites.EnemyScout; }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            Y += Speed;
            sineOffset += Speed * 0.8f;
            X = startX + MathF.Sin(sineOffset * SineFrequency) * SineAmplitude;
            X = Math.Clamp(X, 0, canvasWidth - Width);
        }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.Orange);
            var points = new PointF[] { new(X + Width / 2f, Y), new(X, Y + Height), new(X + Width, Y + Height) };
            g.FillPolygon(brush, points);
        }
    }

    public class ShooterEnemy : Enemy
    {
        private int shootTimer;

        public ShooterEnemy(float x, float y, float speedMultiplier, int hpBonus)
            : base(x, y, GameConfig.ShooterEnemyWidth, GameConfig.ShooterEnemyHeight,
                  GameConfig.ShooterEnemySpeed * speedMultiplier,
                  GameConfig.ShooterEnemyHP + hpBonus,
                  GameConfig.ShooterEnemyScore, GameConfig.ShooterEnemyCoinChance)
        { shootTimer = GameConfig.ShooterEnemyShootIntervalMs / 2; Sprite = Sprites.EnemyShooter; }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            Y += Speed;
            shootTimer += GameConfig.TimerIntervalMs;
        }

        public override bool ShouldShoot() => shootTimer >= GameConfig.ShooterEnemyShootIntervalMs;

        public override Bullet? Shoot()
        {
            shootTimer = 0;
            return new EnemyBullet(X + Width / 2f - GameConfig.EnemyBulletWidth / 2f,
                                   Y + Height, GameConfig.EnemyBulletSpeed);
        }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.Yellow);
            g.FillRectangle(brush, X, Y, Width, Height);
            using var innerBrush = new SolidBrush(Color.DarkGoldenrod);
            g.FillRectangle(innerBrush, X + 4, Y + 4, Width - 8, Height - 8);
        }
    }

    public class HeavyTankEnemy : Enemy
    {
        private int shootTimer;
        private int maxHP;

        public HeavyTankEnemy(float x, float y, float speedMultiplier, int hpBonus)
            : base(x, y, GameConfig.HeavyTankEnemyWidth, GameConfig.HeavyTankEnemyHeight,
                  GameConfig.HeavyTankEnemySpeed * speedMultiplier,
                  GameConfig.HeavyTankEnemyHP + hpBonus,
                  GameConfig.HeavyTankEnemyScore, GameConfig.HeavyTankEnemyCoinChance)
        { maxHP = HP; shootTimer = 0; Sprite = Sprites.EnemyHeavyTank; }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            Y += Speed;
            shootTimer += GameConfig.TimerIntervalMs;
        }

        public override bool ShouldShoot() => shootTimer >= GameConfig.ShooterEnemyShootIntervalMs;
        public override Bullet? Shoot()
        {
            shootTimer = 0;
            float cx = X + Width / 2f - GameConfig.EnemyBulletWidth / 2f;
            float cy = Y + Height;
            float speed = GameConfig.EnemyBulletSpeed * 0.8f;
            return new EnemyBullet(cx, cy, 0, speed);
        }

        public List<Bullet> ShootAll()
        {
            shootTimer = 0;
            var bullets = new List<Bullet>();
            float cx = X + Width / 2f;
            float cy = Y + Height / 2f;
            float speed = GameConfig.EnemyBulletSpeed * 0.8f;
            float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
            foreach (float angleDeg in angles)
            {
                float angleRad = angleDeg * MathF.PI / 180f;
                float vx = MathF.Cos(angleRad) * speed;
                float vy = MathF.Sin(angleRad) * speed;
                float bx = cx + MathF.Cos(angleRad) * (Width / 2f + 5) - GameConfig.EnemyBulletWidth / 2f;
                float by = cy + MathF.Sin(angleRad) * (Height / 2f + 5) - GameConfig.EnemyBulletHeight / 2f;
                bullets.Add(new EnemyBullet(bx, by, vx, vy));
            }
            return bullets;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            float barX = X; float barY = Y - 10;
            using var bgBrush = new SolidBrush(Color.DarkGray);
            g.FillRectangle(bgBrush, barX, barY, Width, 6);
            float hpRatio = (float)HP / maxHP;
            Color hpColor = hpRatio > 0.5f ? Color.Green : (hpRatio > 0.25f ? Color.Yellow : Color.Red);
            using var hpBrush = new SolidBrush(hpColor);
            g.FillRectangle(hpBrush, barX, barY, Width * hpRatio, 6);
            using var borderPen = new Pen(Color.White, 1);
            g.DrawRectangle(borderPen, barX, barY, Width, 6);
        }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.DarkRed);
            g.FillRectangle(brush, X, Y, Width, Height);
            using var borderPen = new Pen(Color.Red, 2);
            g.DrawRectangle(borderPen, X, Y, Width, Height);
        }
    }
}
