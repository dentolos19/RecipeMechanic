using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RecipeMechanic.Models;

namespace RecipeMechanic.Core;

public static class Utilities
{

    public static IList<RecipeDataModel> GetItemRecipes(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<RecipeDataModel>>(fileContent, new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip });
    }

    public static IDictionary<Guid, DescriptionDataModel> GetItemDescriptions(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<Guid, DescriptionDataModel>>(fileContent, new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip });
    }

    public static IList<RecipeItemModel> MergeRecipesAndDescriptions(IEnumerable<RecipeDataModel> recipes, IEnumerable<GameItemModel> items)
    {
        var recipeItems = new List<RecipeItemModel>();
        foreach (var recipe in recipes)
            recipeItems.Add(ConvertDataToItem(recipe, items));
        return recipeItems.ToArray();
    }

    public static RecipeItemModel ConvertDataToItem(RecipeDataModel recipeData, IEnumerable<GameItemModel> items)
    {
        var recipeItem = new RecipeItemModel { Id = recipeData.Id, Data = recipeData };
        foreach (var item in items)
        {
            if (!recipeItem.Id.Equals(item.Id))
                continue;
            recipeItem.Name = item.Name;
            recipeItem.Description = item.Description;
        }
        return recipeItem;
    }

}