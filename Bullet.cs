using System;
using System.Collections.Generic;
using System.Drawing;

namespace AP_Final_Project
{
    public abstract class Bullet : GameObject
    {
        public int Damage { get; }
        public bool IsPlayerBullet { get; }

        protected Bullet(float x, float y, int width, int height, float speed,
                         int damage, bool isPlayerBullet)
            : base(x, y, width, height, speed, 1)
        {
            Damage = damage;
            IsPlayerBullet = isPlayerBullet;
        }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            if (IsPlayerBullet)
                Y -= Speed;
            else
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
            using var brush = new SolidBrush(IsPlayerBullet ? Color.LimeGreen : Color.Red);
            g.FillRectangle(brush, X, Y, Width, Height);
        }
    }

    public class PlayerBullet : Bullet
    {
        public PlayerBullet(float x, float y, float speed, int damage, string bulletType)
            : base(x, y, GameConfig.PlayerBulletWidth, GameConfig.PlayerBulletHeight,
                  speed, damage, true)
        {
            Sprite = bulletType.Contains("Plasma") ? Sprites.BulletPlasma : Sprites.BulletGreenLaser;
        }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.LimeGreen);
            g.FillRectangle(brush, X, Y, Width, Height);
        }
    }

    public class EnemyBullet : Bullet
    {
        public float VelocityX { get; }
        public float VelocityY { get; }

        public EnemyBullet(float x, float y, float speed)
            : base(x, y, GameConfig.EnemyBulletWidth, GameConfig.EnemyBulletHeight,
                  speed, GameConfig.EnemyBulletDamage, false)
        {
            VelocityX = 0;
            VelocityY = speed;
            Sprite = Sprites.BulletEnemy;
        }

        public EnemyBullet(float x, float y, float vx, float vy)
            : base(x, y, GameConfig.EnemyBulletWidth, GameConfig.EnemyBulletHeight,
                  MathF.Sqrt(vx * vx + vy * vy), GameConfig.EnemyBulletDamage, false)
        {
            VelocityX = vx;
            VelocityY = vy;
            Sprite = Sprites.BulletEnemy;
        }

        public override void Update(int canvasWidth, int canvasHeight)
        {
            X += VelocityX;
            Y += VelocityY;
        }

        protected override void DrawPlaceholder(Graphics g)
        {
            using var brush = new SolidBrush(Color.Red);
            g.FillRectangle(brush, X, Y, Width, Height);
        }
    }

    public static class BulletFactory
    {
        public static List<PlayerBullet> CreatePlayerBullets(
            float playerX, float playerY, int playerWidth,
            string bulletType, bool tripleShot)
        {
            var bullets = new List<PlayerBullet>();
            float speed = GameConfig.PlayerBulletSpeed;
            int damage = bulletType.Contains("Plasma") ? GameConfig.PlasmaBulletDamage : GameConfig.PlayerBulletDamage;
            float cx = playerX + playerWidth / 2f - GameConfig.PlayerBulletWidth / 2f;

            bullets.Add(new PlayerBullet(cx, playerY - GameConfig.PlayerBulletHeight, speed, damage, bulletType));

            if (tripleShot)
            {
                bullets.Add(new PlayerBullet(
                    cx - GameConfig.PlayerBulletWidth * 2,
                    playerY - GameConfig.PlayerBulletHeight + 5,
                    speed, damage, bulletType));
                bullets.Add(new PlayerBullet(
                    cx + GameConfig.PlayerBulletWidth * 2,
                    playerY - GameConfig.PlayerBulletHeight + 5,
                    speed, damage, bulletType));
            }

            return bullets;
        }
    }
}
