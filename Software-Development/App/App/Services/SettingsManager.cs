using System.IO;
using System.Text.Json;
using App.Models;

namespace App.Services
{
    public static class SettingsManager
    {
        private static readonly string settingsFile = "settings.json";

        public static Settings LoadSettings()
        {
            if (!File.Exists(settingsFile))
                return new Settings();

            string json = File.ReadAllText(settingsFile);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

        public static void SaveSettings(Settings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsFile, json);
        }
    }
}
