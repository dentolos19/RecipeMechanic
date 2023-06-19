using System.Text.Json.Serialization;

namespace RecipeMechanic.Models;

public class IngredientDataModel
{
    [JsonPropertyName("quantity")] public int Quantity { get; init; }
    [JsonPropertyName("itemId")] public Guid Id { get; init; }
}