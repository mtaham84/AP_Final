using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AP_Final_Project
{
    public static class DatabaseHelper
    {
        private const string connectionString = "Data Source=game_data.db";

        public static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmdPlayer = connection.CreateCommand();
            cmdPlayer.CommandText = @"
                CREATE TABLE IF NOT EXISTS PlayerData (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Coins INTEGER NOT NULL,
                    HighScore INTEGER NOT NULL
                );
                INSERT OR IGNORE INTO PlayerData (Id, Coins, HighScore) VALUES (1, 0, 0);
            ";
            cmdPlayer.ExecuteNonQuery();

            var cmdShop = connection.CreateCommand();
            cmdShop.CommandText = @"
                CREATE TABLE IF NOT EXISTS ShopItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ItemName TEXT NOT NULL,
                    Price INTEGER NOT NULL,
                    IsPurchased INTEGER NOT NULL,
                    IsEquipped INTEGER NOT NULL
                );
            ";
            cmdShop.ExecuteNonQuery();

            EnsureColumn(connection, "ShopItems", "Category", "TEXT DEFAULT 'General'");

            SeedShopItems(connection);
        }

        private static void EnsureColumn(SqliteConnection connection, string table, string column, string definition)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({table})";
            using var reader = cmd.ExecuteReader();
            bool exists = false;
            while (reader.Read())
            {
                if (reader.GetString(1) == column) { exists = true; break; }
            }
            if (!exists)
            {
                using var alter = connection.CreateCommand();
                alter.CommandText = $"ALTER TABLE {table} ADD COLUMN {column} {definition}";
                alter.ExecuteNonQuery();
            }
        }

        private static void SeedShopItems(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR IGNORE INTO ShopItems (Id, ItemName, Price, IsPurchased, IsEquipped, Category)
                VALUES
                    (1, 'Ship Skin - Red Eagle', 500, 0, 0, 'Ship Skin'),
                    (2, 'Bullet Style - Plasma', 300, 0, 0, 'Bullet Style'),
                    (3, 'Background - Galaxy', 400, 0, 0, 'Background Theme'),
                    (5, 'Ship Skin - Cyber Phantom', 750, 0, 0, 'Ship Skin'),
                    (6, 'Bullet Style - Green Laser', 200, 1, 1, 'Bullet Style'),
                    (7, 'Background - Mars', 350, 0, 0, 'Background Theme'),
                    (8, 'Background - Neon City', 600, 0, 0, 'Background Theme'),
                    (9, 'Ship Skin - Default', 0, 1, 1, 'Ship Skin');
            ";
            cmd.ExecuteNonQuery();

            var fixCmd = connection.CreateCommand();
            fixCmd.CommandText = @"
                UPDATE ShopItems SET Category = 'Ship Skin' WHERE ItemName LIKE 'Ship Skin%';
                UPDATE ShopItems SET Category = 'Bullet Style' WHERE ItemName LIKE 'Bullet Style%';
                UPDATE ShopItems SET Category = 'Background Theme' WHERE ItemName LIKE 'Background%';
            ";
            fixCmd.ExecuteNonQuery();
        }

        public static int GetCoins()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Coins FROM PlayerData WHERE Id = 1";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static void AddCoins(int amount)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE PlayerData SET Coins = Coins + @amount WHERE Id = 1";
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.ExecuteNonQuery();
        }

        public static void SpendCoins(int amount)
        {
            if (GetCoins() >= amount)
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE PlayerData SET Coins = Coins - @amount WHERE Id = 1";
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }
        }

        public static int GetHighScore()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT HighScore FROM PlayerData WHERE Id = 1";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static void SetHighScore(int score)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE PlayerData SET HighScore = @score WHERE Id = 1";
            cmd.Parameters.AddWithValue("@score", score);
            cmd.ExecuteNonQuery();
        }

        public static List<ShopItem> GetShopItems()
        {
            var items = new List<ShopItem>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, ItemName, Price, IsPurchased, IsEquipped, Category FROM ShopItems";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ShopItem
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetInt32(2),
                    IsPurchased = reader.GetInt32(3) == 1,
                    IsEquipped = reader.GetInt32(4) == 1,
                    Category = reader.IsDBNull(5) ? "General" : reader.GetString(5)
                });
            }
            return items;
        }

        public static List<ShopItem> GetItemsByCategory(string category)
            => GetShopItems().FindAll(i => i.Category == category);

        public static ShopItem? GetEquippedItemByCategory(string category)
            => GetShopItems().Find(i => i.Category == category && i.IsEquipped);

        public static void PurchaseItem(int itemId)
        {
            var item = GetShopItems().Find(i => i.Id == itemId);
            if (item != null && !item.IsPurchased && GetCoins() >= item.Price)
            {
                SpendCoins(item.Price);
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE ShopItems SET IsPurchased = 1 WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", itemId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void EquipItem(int itemId)
        {
            var item = GetShopItems().Find(i => i.Id == itemId);
            if (item == null) return;
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmdUnequip = connection.CreateCommand();
            cmdUnequip.CommandText = "UPDATE ShopItems SET IsEquipped = 0 WHERE Category = @cat";
            cmdUnequip.Parameters.AddWithValue("@cat", item.Category);
            cmdUnequip.ExecuteNonQuery();
            var cmdEquip = connection.CreateCommand();
            cmdEquip.CommandText = "UPDATE ShopItems SET IsEquipped = 1 WHERE Id = @id";
            cmdEquip.Parameters.AddWithValue("@id", itemId);
            cmdEquip.ExecuteNonQuery();
        }

    }
}
