using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace App.Services
{
    public static class SettingsManager
    {
        // Assuming settings are stored in a JSON file
        private static readonly string SettingsFilePath = "Resources/settings.json";

        // Load settings from the settings.json file
        public static Settings LoadSettings()
        {
            if (!File.Exists(SettingsFilePath))
                return new Settings(); // Return empty settings if file doesn't exist

            string json = File.ReadAllText(SettingsFilePath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

        // Save settings to the settings.json file
        public static void SaveSettings(Settings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
    }

    // ✅ The actual Settings class used by SettingsManager
    public class Settings
    {
        public UserPreferences Preferences { get; set; } = new UserPreferences();
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class UserPreferences
    {
        public string Theme { get; set; } = "Light";
        public string Language { get; set; } = "en";

        // Add GridSize property
        public int GridSize { get; set; } = 5;  // Default grid size, adjust if needed
    }
}

