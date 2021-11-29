using System;
using System.IO;
using System.Text.Json;

namespace RecipeMechanic.Core;

public class Settings
{

    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeMechanic.settings.json");

    public string SavedGamePath { get; set; }

    public void Save()
    {
        var fileContent = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, fileContent);
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        var fileContent = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Settings>(fileContent);
    }

}