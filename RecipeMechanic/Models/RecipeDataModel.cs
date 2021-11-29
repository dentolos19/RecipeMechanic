using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipeMechanic.Models;

public class RecipeDataModel
{

    [JsonPropertyName("itemId")] public Guid Id { get; init; }
    [JsonPropertyName("quantity")] public int OutputQuantity { get; init; }
    [JsonPropertyName("craftTime")] public int CraftingDuration { get; init; }
    [JsonPropertyName("ingredientList")] public IList<IngredientDataModel> Ingredients { get; init; }

}