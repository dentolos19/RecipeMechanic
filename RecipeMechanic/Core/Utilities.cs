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

    public static IList<RecipeItemModel> MergeRecipesAndDescriptions(IEnumerable<RecipeDataModel> recipes, IDictionary<Guid, DescriptionDataModel> descriptions)
    {
        var recipeItems = new List<RecipeItemModel>();
        foreach (var recipe in recipes)
        {
            var recipeItem = new RecipeItemModel { Id = recipe.Id };
            foreach (var (id, description) in descriptions)
            {
                if (!recipe.Id.Equals(id))
                    continue;
                recipeItem.Name = description.Title;
                recipeItem.Description = description.Description;
            }
            recipeItems.Add(recipeItem);
        }
        return recipeItems.ToArray();
    }

}