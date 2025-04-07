using System.IO;
using System.Text.Json;
using App.Models;
using System.Collections.Generic;

namespace App.Services
{
    public class GameStatsService
    {
        private readonly string GameStatsFilePath = "Resources/gamestats.json";

        public void SaveGameStats(List<GameStats> gameStatsList)
        {
            string json = JsonSerializer.Serialize(gameStatsList);
            File.WriteAllText(GameStatsFilePath, json);
        }

        public List<GameStats> LoadGameStats()
        {
            if (!File.Exists(GameStatsFilePath)) return new List<GameStats>();
            string json = File.ReadAllText(GameStatsFilePath);
            return JsonSerializer.Deserialize<List<GameStats>>(json);
        }

        public GameStats GetGameStatsByUsername(string username)
        {
            var gameStatsList = LoadGameStats();
            return gameStatsList.Find(stats => stats.Username == username);
        }

        public void UpdateGameStats(string username, GameStats updatedStats)
        {
            var gameStatsList = LoadGameStats();
            var stats = gameStatsList.Find(s => s.Username == username);
            if (stats != null)
            {
                stats.CompletedPuzzles = updatedStats.CompletedPuzzles;
                stats.TotalPlayTimeMinutes = updatedStats.TotalPlayTimeMinutes;
            }
            SaveGameStats(gameStatsList);
        }
    }
}
