using System.IO;
using System.Text.Json;

namespace RecipeMechanic.Core;

public class Settings
{
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeMechanic.settings.json");

    public string? GamePath { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Settings>(json)!;
        }
        catch
        {
            return new Settings();
        }
    }
}