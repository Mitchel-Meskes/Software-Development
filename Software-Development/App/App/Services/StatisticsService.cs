using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using App.Models;
using App.Tests.Utils;
using App.Utils;  // Verplaatst TestFileHelper naar hoofdproject onder App/Utils

namespace App.Services
{
    public static class StatisticsService
    {
        private static readonly string statisticsFilePath = TestFileHelper.GetTestFilePath("statistics.json");

        public static List<GameStats> LoadStats()
        {
            if (!File.Exists(statisticsFilePath) || new FileInfo(statisticsFilePath).Length == 0)
                return new List<GameStats>();

            string json = File.ReadAllText(statisticsFilePath);
            return JsonSerializer.Deserialize<List<GameStats>>(json) ?? new List<GameStats>();
        }

        public static void SaveStats(List<GameStats> stats)
        {
            string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });

            string? dir = Path.GetDirectoryName(statisticsFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(statisticsFilePath, json);
        }

        public static void UpdateStats(string username, double playTimeMinutes)
        {
            var stats = LoadStats();
            var player = stats.FirstOrDefault(s => s.Username == username);

            if (player == null)
                stats.Add(new GameStats(username, 1, playTimeMinutes));
            else
            {
                player.CompletedPuzzles++;
                player.TotalPlayTimeMinutes += playTimeMinutes;
            }

            SaveStats(stats);
        }
    }
}
