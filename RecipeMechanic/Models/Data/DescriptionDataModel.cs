using System.Text.Json.Serialization;

namespace RecipeMechanic.Models;

public class DescriptionDataModel
{

    [JsonPropertyName("description")] public string? Description { get; init; }
    [JsonPropertyName("title")] public string Title { get; init; }

}