using System;
using System.Collections.Generic;

namespace AP_Final_Project.Managers
{
    public class CollisionManager
    {
        public CollisionResult DetectAll(
            Player player,
            List<Enemy> enemies,
            List<Bullet> bullets,
            List<Coin> coins,
            List<PowerUp> powerUps)
        {
            var result = new CollisionResult();

            for (int b = bullets.Count - 1; b >= 0; b--)
            {
                if (!bullets[b].IsPlayerBullet) continue;
                for (int e = enemies.Count - 1; e >= 0; e--)
                {
                    if (!enemies[e].IsAlive) continue;
                    if (bullets[b].Intersects(enemies[e]))
                    {
                        enemies[e].HP -= bullets[b].Damage;
                        bullets[b].HP = 0;
                        result.DestroyedBullets.Add(bullets[b]);
                        if (!enemies[e].IsAlive)
                        {
                            result.DestroyedEnemies.Add(enemies[e]);
                            result.ScoreGained += enemies[e].ScoreValue;
                            result.EnemiesKilled.Add(enemies[e]);
                        }
                        break;
                    }
                }
            }

            for (int b = bullets.Count - 1; b >= 0; b--)
            {
                if (bullets[b].IsPlayerBullet) continue;
                if (bullets[b].Intersects(player))
                {
                    bullets[b].HP = 0;
                    result.DestroyedBullets.Add(bullets[b]);
                    result.PlayerDamage += bullets[b].Damage;
                }
            }

            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive) continue;
                if (enemy.Intersects(player))
                {
                    ResolvePlayerEnemyOverlap(player, enemy);
                    player.IsTouchingEnemy = true;
                }
            }

            for (int c = coins.Count - 1; c >= 0; c--)
            {
                if (coins[c].Intersects(player))
                {
                    coins[c].HP = 0;
                    result.DestroyedCoins.Add(coins[c]);
                    result.CoinsGained += coins[c].Value;
                }
            }

            for (int p = powerUps.Count - 1; p >= 0; p--)
            {
                if (powerUps[p].Intersects(player))
                {
                    powerUps[p].HP = 0;
                    result.DestroyedPowerUps.Add(powerUps[p]);
                    result.ActivePowerUps.Add(powerUps[p]);
                }
            }

            return result;
        }

        private static void ResolvePlayerEnemyOverlap(Player player, Enemy enemy)
        {
            float overlapLeft = (player.X + player.Width) - enemy.X;
            float overlapRight = (enemy.X + enemy.Width) - player.X;
            float overlapTop = (player.Y + player.Height) - enemy.Y;
            float overlapBottom = (enemy.Y + enemy.Height) - player.Y;

            float minOverlap = Math.Min(Math.Min(overlapLeft, overlapRight), Math.Min(overlapTop, overlapBottom));

            if (minOverlap == overlapLeft) player.X -= overlapLeft;
            else if (minOverlap == overlapRight) player.X += overlapRight;
            else if (minOverlap == overlapTop) player.Y -= overlapTop;
            else player.Y += overlapBottom;
        }

        public class CollisionResult
        {
            public List<Bullet> DestroyedBullets { get; } = new();
            public List<Enemy> DestroyedEnemies { get; } = new();
            public List<Coin> DestroyedCoins { get; } = new();
            public List<PowerUp> DestroyedPowerUps { get; } = new();
            public List<Enemy> EnemiesKilled { get; } = new();
            public List<PowerUp> ActivePowerUps { get; } = new();
            public int ScoreGained { get; set; }
            public int CoinsGained { get; set; }
            public int PlayerDamage { get; set; }
        }
    }
}
