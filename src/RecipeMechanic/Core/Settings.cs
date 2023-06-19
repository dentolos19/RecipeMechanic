using System.IO;
using System.Text.Json;

namespace RecipeMechanic.Core;

public class Settings
{
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeMechanic.settings.json");

    public string GamePath { get; set; }

    public void Save()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        return JsonSerializer.Deserialize<Settings>(File.ReadAllText(FilePath));
    }
}