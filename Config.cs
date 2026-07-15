namespace AP_Final_Project
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsEquipped { get; set; }
        public string Category { get; set; } = "General";
    }

    public static class GameConfig
    {
        public const int CanvasWidth = 800;
        public const int CanvasHeight = 600;
        public const int TimerIntervalMs = 20;

        public const int PlayerBaseHP = 5;
        public const int PlayerBaseLives = 3;
        public const float PlayerBaseSpeed = 5f;
        public const int PlayerWidth = 40;
        public const int PlayerHeight = 48;
        public const int FireRateCooldownMs = 200;
        public const int FireRateBoostedMs = 100;

        public const float PlayerBulletSpeed = 8f;
        public const float EnemyBulletSpeed = 6f;
        public const int PlayerBulletDamage = 1;
        public const int PlasmaBulletDamage = 2;
        public const int EnemyBulletDamage = 1;
        public const int PlayerBulletWidth = 6;
        public const int PlayerBulletHeight = 14;
        public const int EnemyBulletWidth = 6;
        public const int EnemyBulletHeight = 10;

        public const int SilverCoinValue = 1;
        public const int GoldCoinValue = 5;
        public const int SilverCoinSize = 18;
        public const int GoldCoinSize = 22;
        public const float CoinFallSpeed = 2f;

        public const int PowerUpSize = 28;
        public const float PowerUpFallSpeed = 2f;
        public const int TripleShotDurationMs = 10000;
        public const int FireRateDurationMs = 10000;

        public const int StandardEnemyHP = 1;
        public const float StandardEnemySpeed = 2f;
        public const int StandardEnemyScore = 100;
        public const int StandardEnemyWidth = 36;
        public const int StandardEnemyHeight = 36;

        public const int ScoutEnemyHP = 1;
        public const float ScoutEnemySpeed = 4.5f;
        public const int ScoutEnemyScore = 150;
        public const int ScoutEnemyWidth = 30;
        public const int ScoutEnemyHeight = 30;

        public const int ShooterEnemyHP = 2;
        public const float ShooterEnemySpeed = 1.5f;
        public const int ShooterEnemyScore = 200;
        public const int ShooterEnemyWidth = 38;
        public const int ShooterEnemyHeight = 38;
        public const int ShooterEnemyShootIntervalMs = 1500;

        public const int HeavyTankEnemyHP = 5;
        public const float HeavyTankEnemySpeed = 0.8f;
        public const int HeavyTankEnemyScore = 500;
        public const int HeavyTankEnemyWidth = 50;
        public const int HeavyTankEnemyHeight = 50;

        public const int StandardEnemyCoinChance = 60;
        public const int ScoutEnemyCoinChance = 50;
        public const int ShooterEnemyCoinChance = 70;
        public const int HeavyTankEnemyCoinChance = 90;

        public const int EnemyPowerUpDropChance = 15;
        public const int ContactDamageIntervalMs = 500;

        public const int WaveAnnouncementDurationMs = 2000;
        public const int SpawnDelayMs = 600;

        public static readonly (int std, int scout, int shooter, int heavy)[] WaveDefinitions =
        {
            (8, 0, 0, 0),
            (6, 4, 0, 0),
            (5, 3, 2, 0),
            (4, 2, 4, 0),
            (3, 3, 3, 1),
            (4, 3, 3, 1),
            (5, 5, 4, 2),
            (4, 4, 5, 4),
            (3, 5, 6, 5),
            (4, 6, 7, 7),
        };

        public const int TotalWaves = 10;
    }
}
