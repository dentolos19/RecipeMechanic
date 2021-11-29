using System;
using System.Text.Json.Serialization;

namespace RecipeMechanic.Models;

public class IngredientDataModel
{

    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("itemId")] public Guid Id { get; set; }

}