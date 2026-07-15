using System;
using System.Drawing;

namespace AP_Final_Project
{
    public abstract class GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public float Speed { get; set; }
        public int HP { get; set; }
        public bool IsAlive => HP > 0;
        public Bitmap? Sprite { get; set; }
        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        protected GameObject(float x, float y, int width, int height, float speed, int hp)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            HP = hp;
        }

        public abstract void Update(int canvasWidth, int canvasHeight);
        public abstract void Draw(Graphics g);
        public bool Intersects(GameObject other) => Bounds.IntersectsWith(other.Bounds);
        public bool IsOffScreen(int canvasWidth, int canvasHeight)
            => X + Width < 0 || X > canvasWidth || Y + Height < 0 || Y > canvasHeight;
    }

    public class Player : GameObject
    {
        public int MaxHP { get; }
        public int Lives { get; set; }
        public int Score { get; set; }
        public int CoinsCollected { get; set; }
        public string EquippedSkin { get; set; }
        public string EquippedBulletType { get; set; }
        public int TripleShotTimer { get; set; }
        public int FireRateTimer { get; set; }
        public int ShootCooldown => FireRateTimer > 0 ? GameConfig.FireRateBoostedMs : GameConfig.FireRateCooldownMs;
        private int shootCooldownCounter;

        public bool IsTouchingEnemy { get; set; }
        private int contactDamageCooldown;

        public Player(float x, float y, string skin, string bulletType)
            : base(x, y, GameConfig.PlayerWidth, GameConfig.PlayerHeight,
                  GameConfig.PlayerBaseSpeed, GameConfig.PlayerBaseHP)
        {
            MaxHP = GameConfig.PlayerBaseHP;
            Lives = GameConfig.PlayerBaseLives;
            EquippedSkin = skin;
            EquippedBulletType = bulletType;
            Score = 0;
            CoinsCollected = 0;
            shootCooldownCounter = 0;
            contactDamageCooldown = 0;
        }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            if (shootCooldownCounter > 0)
                shootCooldownCounter -= GameConfig.TimerIntervalMs;
        }

        public override void Draw(Graphics g)
        {
            Bitmap? sprite = GetSkinSprite();
            if (sprite != null)
                g.DrawImage(sprite, X, Y, Width, Height);
            else
            {
                using var brush = new SolidBrush(Color.White);
                var points = new PointF[]
                {
                    new(X + Width / 2f, Y),
                    new(X, Y + Height),
                    new(X + Width, Y + Height)
                };
                g.FillPolygon(brush, points);
            }
        }

        private Bitmap? GetSkinSprite()
        {
            if (EquippedSkin.Contains("Red Eagle")) return Sprites.PlayerRedEagle;
            if (EquippedSkin.Contains("Cyber Phantom")) return Sprites.PlayerCyberPhantom;
            return Sprites.PlayerDefault;
        }

        public void Move(float dx, float dy, int canvasWidth, int canvasHeight)
        {
            X += dx * Speed;
            Y += dy * Speed;
            X = Math.Clamp(X, 0, canvasWidth - Width);
            Y = Math.Clamp(Y, 0, canvasHeight - Height);
        }

        public bool CanShoot() => shootCooldownCounter <= 0;

        public void ResetShootCooldown() => shootCooldownCounter = ShootCooldown;

        public void TakeDamage(int amount)
        {
            HP -= amount;
            if (HP < 0) HP = 0;
        }

        public void UpdatePowerUpTimers(int deltaMs)
        {
            if (TripleShotTimer > 0)
            {
                TripleShotTimer -= deltaMs;
                if (TripleShotTimer <= 0) TripleShotTimer = 0;
            }

            if (FireRateTimer > 0)
            {
                FireRateTimer -= deltaMs;
                if (FireRateTimer <= 0) FireRateTimer = 0;
            }
        }

        public void UpdateContactDamage(int deltaMs)
        {
            if (IsTouchingEnemy)
            {
                contactDamageCooldown -= deltaMs;
                if (contactDamageCooldown <= 0)
                {
                    HP -= 1;
                    if (HP < 0) HP = 0;
                    contactDamageCooldown = GameConfig.ContactDamageIntervalMs;
                }
            }
            else
            {
                contactDamageCooldown = 0;
            }
        }
    }

    public enum CoinType { Silver, Gold }

    public class Coin : GameObject
    {
        public CoinType Type { get; }
        public int Value => Type == CoinType.Gold ? GameConfig.GoldCoinValue : GameConfig.SilverCoinValue;

        public Coin(float x, float y, CoinType type)
            : base(x, y,
                  type == CoinType.Gold ? GameConfig.GoldCoinSize : GameConfig.SilverCoinSize,
                  type == CoinType.Gold ? GameConfig.GoldCoinSize : GameConfig.SilverCoinSize,
                  GameConfig.CoinFallSpeed, 1)
        {
            Type = type;
        }

        public override void Update(int canvasWidth, int canvasHeight) => Y += Speed;

        public override void Draw(Graphics g)
        {
            Color fillColor = Type == CoinType.Gold ? Color.Gold : Color.LightGray;
            Color borderColor = Type == CoinType.Gold ? Color.DarkGoldenrod : Color.Gray;
            using var brush = new SolidBrush(fillColor);
            using var pen = new Pen(borderColor, 2);
            g.FillEllipse(brush, X, Y, Width, Height);
            g.DrawEllipse(pen, X, Y, Width, Height);

            string label = Type == CoinType.Gold ? "G" : "S";
            using var font = new Font("Arial", 9, FontStyle.Bold);
            var textSize = g.MeasureString(label, font);
            g.DrawString(label, font, Brushes.Black,
                X + (Width - textSize.Width) / 2,
                Y + (Height - textSize.Height) / 2);
        }
    }
}
