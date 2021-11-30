using RecipeMechanic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Xml;

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

    public static IDictionary<Guid, Bitmap> GetIconsFromMap(string iconMapPath, string iconPointsPath)
    {
        var document = new XmlDocument();
        document.LoadXml(File.ReadAllText(iconPointsPath));
        var nodes = document.SelectNodes("//Group/Index");
        var iconMap = new Bitmap(iconMapPath);
        var iconSize = new Size(96, 96);
        var icons = new Dictionary<Guid, Bitmap>();
        foreach (XmlNode node in nodes)
        {
            if (!Guid.TryParse(node.Attributes["name"].Value, out var id))
                continue;
            var pointValues = node.SelectSingleNode("./Frame").Attributes["point"].Value.Split(' ');
            var iconPoint = new Point(int.Parse(pointValues[0]), int.Parse(pointValues[1]));
            var iconArea = new Rectangle(iconPoint, iconSize);
            var icon = iconMap.Clone(iconArea, iconMap.PixelFormat);
            icons.Add(id, icon);
        }
        return icons;
    }

}