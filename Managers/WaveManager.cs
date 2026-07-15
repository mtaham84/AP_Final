using System;
using System.Collections.Generic;

namespace AP_Final_Project.Managers
{
    public class WaveManager
    {
        public int CurrentWave { get; private set; }
        public string WaveAnnouncement { get; private set; } = "";
        public int AnnouncementTimer { get; private set; }

        private int spawnTimer;
        private int spawnIndex;
        private List<SpawnEntry> currentSchedule;
        private bool waveInProgress;
        private Random rng;

        public WaveManager()
        {
            CurrentWave = 0;
            waveInProgress = false;
            currentSchedule = new List<SpawnEntry>();
            rng = new Random();
            AnnouncementTimer = 0;
        }

        public void StartNextWave()
        {
            if (CurrentWave >= GameConfig.TotalWaves)
                return;

            CurrentWave++;
            waveInProgress = true;
            spawnIndex = 0;
            spawnTimer = 0;
            BuildSchedule(CurrentWave);
            WaveAnnouncement = $"Wave {CurrentWave}!";
            AnnouncementTimer = GameConfig.WaveAnnouncementDurationMs;
        }

        public bool IsWaveCleared(int aliveEnemyCount)
        {
            return waveInProgress && spawnIndex >= currentSchedule.Count && aliveEnemyCount == 0;
        }

        public List<Enemy> Update(int deltaMs, int canvasWidth)
        {
            var spawned = new List<Enemy>();

            if (AnnouncementTimer > 0)
            {
                AnnouncementTimer -= deltaMs;
                if (AnnouncementTimer < 0) AnnouncementTimer = 0;
                return spawned;
            }

            if (!waveInProgress || spawnIndex >= currentSchedule.Count)
                return spawned;

            spawnTimer += deltaMs;
            var entry = currentSchedule[spawnIndex];

            if (spawnTimer >= entry.DelayMs)
            {
                spawnTimer = 0;
                float x = entry.SpawnX >= 0 ? entry.SpawnX : rng.Next(50, canvasWidth - 50);

                int wave = CurrentWave;
                float speedMult = 1f + 0.1f * wave;
                int hpBonus = 2 * wave;

                Enemy enemy = entry.Type switch
                {
                    EnemyType.Standard => new StandardEnemy(x, -40, speedMult, hpBonus),
                    EnemyType.Scout => new ScoutEnemy(x, -40, speedMult, hpBonus),
                    EnemyType.Shooter => new ShooterEnemy(x, -40, speedMult, hpBonus),
                    EnemyType.HeavyTank => new HeavyTankEnemy(x, -40, speedMult, hpBonus),
                    _ => new StandardEnemy(x, -40, speedMult, hpBonus)
                };

                spawned.Add(enemy);
                spawnIndex++;
            }

            return spawned;
        }

        private void BuildSchedule(int waveNumber)
        {
            currentSchedule.Clear();
            if (waveNumber < 1 || waveNumber > GameConfig.TotalWaves) return;

            var def = GameConfig.WaveDefinitions[waveNumber - 1];
            int delay = GameConfig.SpawnDelayMs;

            for (int i = 0; i < def.std; i++)
                currentSchedule.Add(new SpawnEntry(EnemyType.Standard, -1, delay));
            for (int i = 0; i < def.scout; i++)
                currentSchedule.Add(new SpawnEntry(EnemyType.Scout, -1, delay));
            for (int i = 0; i < def.shooter; i++)
                currentSchedule.Add(new SpawnEntry(EnemyType.Shooter, -1, delay));
            for (int i = 0; i < def.heavy; i++)
                currentSchedule.Add(new SpawnEntry(EnemyType.HeavyTank, -1, delay + 400));

            // Shuffle for variety
            for (int i = currentSchedule.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (currentSchedule[i], currentSchedule[j]) = (currentSchedule[j], currentSchedule[i]);
            }

            foreach (var entry in currentSchedule)
            {
                if (entry.Type == EnemyType.HeavyTank)
                    entry.DelayMs = delay + 300;
            }
        }

        private enum EnemyType { Standard, Scout, Shooter, HeavyTank }

        private class SpawnEntry
        {
            public EnemyType Type;
            public int SpawnX;
            public int DelayMs;

            public SpawnEntry(EnemyType type, int spawnX, int delayMs)
            {
                Type = type;
                SpawnX = spawnX;
                DelayMs = delayMs;
            }
        }
    }
}
