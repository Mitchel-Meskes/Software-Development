using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace App.Services
{
    public class GameStats
    {
        public string Username { get; set; }
        public int CompletedPuzzles { get; set; }
        public double TotalPlayTimeMinutes { get; set; }
    }

    public static class StatisticsService
    {
        private static readonly string FilePath = "Resources/statistics.json";

        public static List<GameStats> LoadStats()
        {
            if (!File.Exists(FilePath)) return new List<GameStats>();
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<GameStats>>(json);
        }

        public static void SaveStats(List<GameStats> stats)
        {
            string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

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
