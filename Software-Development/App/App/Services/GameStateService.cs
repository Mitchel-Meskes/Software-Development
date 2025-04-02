using System.IO;
using System.Text.Json;
using App.Models;

namespace App.Services
{
    public static class GameStateService
    {
        private static readonly string FilePath = "Resources/savegame.json";

        public static void Save(NonogramPuzzle puzzle)
        {
            string json = JsonSerializer.Serialize(puzzle);
            File.WriteAllText(FilePath, json);
        }

        public static NonogramPuzzle Load()
        {
            if (!File.Exists(FilePath)) return null;
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<NonogramPuzzle>(json);
        }
    }
}
