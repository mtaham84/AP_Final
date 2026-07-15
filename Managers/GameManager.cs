using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AP_Final_Project.Managers
{
    public class GameManager
    {
        private readonly Player player;
        private readonly List<Enemy> enemies;
        private readonly List<Bullet> bullets;
        private readonly List<Coin> coins;
        private readonly List<PowerUp> powerUps;
        private readonly Random rng;

        private readonly InputManager input;
        private readonly WaveManager waveManager;
        private readonly CollisionManager collisionManager;
        private readonly RenderManager renderManager;

        private bool isGameOver;
        private bool isPaused;
        private readonly Form gameForm;

        public bool IsGameOver => isGameOver;
        public bool IsPaused => isPaused;

        public GameManager(Form form, string equippedSkin, string equippedBullet,
                           string equippedBackground)
        {
            gameForm = form;
            rng = new Random();

            input = new InputManager();
            waveManager = new WaveManager();
            collisionManager = new CollisionManager();
            renderManager = new RenderManager(equippedBackground);

            enemies = new List<Enemy>();
            bullets = new List<Bullet>();
            coins = new List<Coin>();
            powerUps = new List<PowerUp>();

            float startX = GameConfig.CanvasWidth / 2f - GameConfig.PlayerWidth / 2f;
            float startY = GameConfig.CanvasHeight - GameConfig.PlayerHeight - 20;
            player = new Player(startX, startY, equippedSkin, equippedBullet);

            isGameOver = false;
            isPaused = false;

            waveManager.StartNextWave();
        }

        public void OnKeyDown(Keys key) => input.OnKeyDown(key);
        public void OnKeyUp(Keys key) => input.OnKeyUp(key);

        public void Tick(Graphics g, int canvasWidth, int canvasHeight)
        {
            if (isGameOver)
            {
                RenderFrame(g, canvasWidth, canvasHeight, isGameOver: true);
                if (input.PausePressed)
                {
                    input.ConsumePause();
                    gameForm.Close();
                }
                return;
            }

            if (input.PausePressed)
            {
                input.ConsumePause();
                isPaused = !isPaused;
            }

            if (isPaused)
            {
                RenderFrame(g, canvasWidth, canvasHeight, isPaused: true);
                return;
            }

            UpdatePlayerMovement(canvasWidth, canvasHeight);
            HandlePlayerShooting();
            UpdateAllEnemies(canvasWidth, canvasHeight);
            UpdateAllBullets(canvasWidth, canvasHeight);
            UpdateAllCoins(canvasWidth, canvasHeight);
            UpdateAllPowerUps(canvasWidth, canvasHeight);
            player.Update(canvasWidth, canvasHeight);
            player.UpdatePowerUpTimers(GameConfig.TimerIntervalMs);

            var spawned = waveManager.Update(GameConfig.TimerIntervalMs, canvasWidth);
            enemies.AddRange(spawned);

            HandleEnemyShooting();

            player.IsTouchingEnemy = false;

            var collisionResult = collisionManager.DetectAll(
                player, enemies, bullets, coins, powerUps);

            ApplyCollisionResult(collisionResult);
            player.UpdateContactDamage(GameConfig.TimerIntervalMs);

            SpawnDropsFromKilledEnemies(collisionResult);

            RemoveDestroyedObjects();

            CheckWaveClear();
            CheckPlayerDeath();

            RenderFrame(g, canvasWidth, canvasHeight);
        }

        private void UpdatePlayerMovement(int canvasWidth, int canvasHeight)
        {
            float dx = 0, dy = 0;
            if (input.MoveLeft) dx -= 1;
            if (input.MoveRight) dx += 1;
            if (input.MoveUp) dy -= 1;
            if (input.MoveDown) dy += 1;

            if (dx != 0 || dy != 0)
            {
                float mag = MathF.Sqrt(dx * dx + dy * dy);
                dx /= mag;
                dy /= mag;
            }

            player.Move(dx, dy, canvasWidth, canvasHeight);
        }

        private void HandlePlayerShooting()
        {
            if (!input.Shooting || !player.CanShoot()) return;

            var newBullets = BulletFactory.CreatePlayerBullets(
                player.X, player.Y, player.Width,
                player.EquippedBulletType,
                player.TripleShotTimer > 0);

            bullets.AddRange(newBullets);
            player.ResetShootCooldown();
        }

        private void UpdateAllEnemies(int canvasWidth, int canvasHeight)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.IsAlive)
                    enemy.Update(canvasWidth, canvasHeight);
            }
        }

        private void UpdateAllBullets(int canvasWidth, int canvasHeight)
        {
            foreach (var bullet in bullets)
                bullet.Update(canvasWidth, canvasHeight);
        }

        private void UpdateAllCoins(int canvasWidth, int canvasHeight)
        {
            foreach (var coin in coins)
                coin.Update(canvasWidth, canvasHeight);
        }

        private void UpdateAllPowerUps(int canvasWidth, int canvasHeight)
        {
            foreach (var pu in powerUps)
                pu.Update(canvasWidth, canvasHeight);
        }

        private void HandleEnemyShooting()
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive) continue;

                if (enemy is HeavyTankEnemy heavy && heavy.ShouldShoot())
                {
                    bullets.AddRange(heavy.ShootAll());
                }
                else if (enemy.ShouldShoot())
                {
                    var bullet = enemy.Shoot();
                    if (bullet != null) bullets.Add(bullet);
                }
            }
        }

        private void ApplyCollisionResult(CollisionManager.CollisionResult result)
        {
            player.Score += result.ScoreGained;
            player.CoinsCollected += result.CoinsGained;

            if (result.PlayerDamage > 0)
                player.TakeDamage(result.PlayerDamage);

            foreach (var pu in result.ActivePowerUps)
                ApplyPowerUp(pu);
        }

        private void ApplyPowerUp(PowerUp pu)
        {
            switch (pu.Type)
            {
                case PowerUpType.TripleShot:
                    player.TripleShotTimer = GameConfig.TripleShotDurationMs;
                    break;
                case PowerUpType.HealthPack:
                    if (player.Lives < GameConfig.PlayerBaseLives)
                        player.Lives++;
                    else
                        player.HP = player.MaxHP;
                    break;
                case PowerUpType.FireRate:
                    player.FireRateTimer = GameConfig.FireRateDurationMs;
                    break;
            }
        }

        private void SpawnDropsFromKilledEnemies(CollisionManager.CollisionResult result)
        {
            foreach (var killed in result.EnemiesKilled)
            {
                var droppedCoin = killed.DropCoin(rng);
                if (droppedCoin != null) coins.Add(droppedCoin);

                if (rng.Next(100) < GameConfig.EnemyPowerUpDropChance)
                {
                    var pu = CreateRandomPowerUp(
                        killed.X + killed.Width / 2f,
                        killed.Y + killed.Height);
                    if (pu != null) powerUps.Add(pu);
                }
            }
        }

        private PowerUp CreateRandomPowerUp(float x, float y)
        {
            int roll = rng.Next(100);
            if (roll < 35) return new TripleShotPowerUp(x, y);
            if (roll < 70) return new HealthPackPowerUp(x, y);
            return new FireRatePowerUp(x, y);
        }

        private void RemoveDestroyedObjects()
        {
            bullets.RemoveAll(b => !b.IsAlive || b.IsOffScreen(GameConfig.CanvasWidth, GameConfig.CanvasHeight));
            enemies.RemoveAll(e => !e.IsAlive || e.Y > GameConfig.CanvasHeight + 50);
            coins.RemoveAll(c => !c.IsAlive || c.IsOffScreen(GameConfig.CanvasWidth, GameConfig.CanvasHeight));
            powerUps.RemoveAll(p => !p.IsAlive || p.IsOffScreen(GameConfig.CanvasWidth, GameConfig.CanvasHeight));
        }

        private void CheckWaveClear()
        {
            if (!waveManager.IsWaveCleared(enemies.Count)) return;

            if (waveManager.CurrentWave >= GameConfig.TotalWaves)
                EndGame();
            else
                waveManager.StartNextWave();
        }

        private void CheckPlayerDeath()
        {
            if (player.HP > 0) return;

            player.Lives--;
            if (player.Lives <= 0)
            {
                EndGame();
            }
            else
            {
                player.HP = player.MaxHP;
            }
        }

        private void EndGame()
        {
            isGameOver = true;

            int highScore = DatabaseHelper.GetHighScore();
            if (player.Score > highScore)
                DatabaseHelper.SetHighScore(player.Score);

            DatabaseHelper.AddCoins(player.CoinsCollected);
        }

        private void RenderFrame(Graphics g, int width, int height,
            bool isPaused = false, bool isGameOver = false)
        {
            renderManager.Render(g, width, height,
                player, enemies, bullets, coins, powerUps,
                waveManager.CurrentWave, isPaused, isGameOver,
                isGameOver ? player.Score : 0,
                isGameOver ? DatabaseHelper.GetHighScore() : 0,
                waveManager.WaveAnnouncement,
                waveManager.AnnouncementTimer);
        }
    }
}
