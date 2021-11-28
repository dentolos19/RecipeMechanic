using System;
using System.IO;
using System.Text.Json;

namespace RecipeMechanic.Core;

public class Settings
{

    private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeMechanic.settings.json");

    public string SavedGamePath { get; set; }

    public void Save()
    {
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static Settings Load()
    {
        return !File.Exists(SettingsPath)
            ? new Settings()
            : JsonSerializer.Deserialize<Settings>(File.ReadAllText(SettingsPath));
    }

}