using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RecipeMechanic.Models;

namespace RecipeMechanic.Core;

public static class Utilities
{
    public static IEnumerable<RecipeDataModel> GetRecipes(string filePath)
    {
        var recipeData = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<RecipeDataModel>>(recipeData, new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip });
    }

    public static IDictionary<Guid, DescriptionDataModel> GetDescriptions(string filePath)
    {
        var descriptionData = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<Guid, DescriptionDataModel>>(descriptionData, new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip });
    }

    public static IEnumerable<RecipeItemModel> MergeRecipesAndDescriptions(IEnumerable<RecipeDataModel> recipes, IEnumerable<GameItemModel> items)
    {
        var recipeItems = new List<RecipeItemModel>();
        foreach (var recipe in recipes)
            recipeItems.Add(ConvertToRecipeItem(recipe, items));
        return recipeItems;
    }

    public static RecipeItemModel ConvertToRecipeItem(RecipeDataModel recipe, IEnumerable<GameItemModel> items)
    {
        var recipeItem = new RecipeItemModel { Id = recipe.Id.Value, Data = recipe };
        foreach (var item in items)
        {
            if (!recipeItem.Id.Equals(item.Id))
                continue;
            recipeItem.Name = item.Name;
            recipeItem.Description = item.Description;
        }
        return recipeItem;
    }

    public static IngredientItemModel ConvertToIngredientItem(IngredientDataModel ingredient, IEnumerable<GameItemModel> items)
    {
        var ingredientItem = new IngredientItemModel { Id = ingredient.Id, Quantity = ingredient.Quantity, Data = ingredient };
        foreach (var item in items)
        {
            if (!ingredientItem.Id.Equals(item.Id))
                continue;
            ingredientItem.Name = item.Name;
        }
        return ingredientItem;
    }
}