using System;
using System.IO;
using System.Collections.Generic;
using App.Models;  // Adjust the namespace as needed for your project
using System.Linq;

namespace App.Services
{
    public static class StatisticsService
    {
        private static readonly string statisticsFilePath = "Resources/statistics.json";

        // Load the statistics
        public static List<GameStats> LoadStats()
        {
            if (!File.Exists(statisticsFilePath)) return new List<GameStats>();
            string json = File.ReadAllText(statisticsFilePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<GameStats>>(json);
        }

        // Save the statistics
        public static void SaveStats(List<GameStats> stats)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(stats, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(statisticsFilePath, json);
        }

        // Update statistics for a user
        public static void UpdateStats(string username, double playTimeMinutes)
        {
            var stats = LoadStats();
            var player = stats.FirstOrDefault(s => s.Username == username);

            if (player == null)
            {
                stats.Add(new GameStats { Username = username, CompletedPuzzles = 1, TotalPlayTimeMinutes = playTimeMinutes });
            }
            else
            {
                player.CompletedPuzzles++;
                player.TotalPlayTimeMinutes += playTimeMinutes;
            }

            SaveStats(stats);
        }
    }
}
