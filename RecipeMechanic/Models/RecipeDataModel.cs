using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipeMechanic.Models;

public class RecipeDataModel
{

    [JsonPropertyName("itemId")] public Guid? Id { get; set; }
    [JsonPropertyName("quantity")] public int? OutputQuantity { get; set; }
    [JsonPropertyName("craftTime")] public int? CraftingDuration { get; set; }
    [JsonPropertyName("ingredientList")] public IList<IngredientDataModel>? Ingredients { get; set; }

}