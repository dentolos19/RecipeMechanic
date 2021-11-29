using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RecipeMechanic.Models;

namespace RecipeMechanic.Views;

public partial class ModifyRecipeWindow
{

    private GameItemModel[] _items;

    public RecipeDataModel? Recipe { get; private set; }

    public ModifyRecipeWindow(GameItemModel[] items, RecipeDataModel? recipe = null)
    {
        _items = items;
        Recipe = recipe;
        InitializeComponent();
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        foreach (var item in _items)
            OutputItemSelection.Items.Add(item);
        if (Recipe is not null)
        {
            OutputItemSelection.SelectedItem = _items.FirstOrDefault(item => item.Id.Equals(Recipe.Id));
            OutputQuantityInput.Value = Recipe.OutputQuantity;
            CraftingDurationInput.Value = Recipe.CraftingDuration;
            foreach (var ingredient in Recipe.Ingredients)
            {
                var item = _items.FirstOrDefault(item => item.Id.Equals(ingredient.Id));
                if (item is not null)
                    IngredientList.Items.Add(new IngredientItemModel
                    {
                        Id = ingredient.Id,
                        Name = item.Name,
                        Quantity = ingredient.Quantity
                    });
            }
        }
        else
        {
            Title = "New Recipe";
        }
        if (OutputItemSelection.SelectedItem is null)
            OutputItemSelection.SelectedIndex = new Random().Next(OutputItemSelection.Items.Count - 1);
    }

    private void OnAddIngredient(object sender, RoutedEventArgs args)
    {
        // TODO: add ingredient
    }

    private void OnModifyIngredient(object sender, RoutedEventArgs args)
    {
        // TODO: add ingredient
    }

    private void OnRemoveIngredient(object sender, RoutedEventArgs args)
    {
        // TODO: add ingredient
    }

    private void OnContinue(object sender, RoutedEventArgs args)
    {
        if (OutputItemSelection.SelectedItem is not GameItemModel item)
        {
            MessageBox.Show("You must select a valid output item for the recipe!", "Recipe Mechanic");
            return;
        }
        if (!(IngredientList.Items.Count > 0))
        {
            MessageBox.Show("You must at least have one ingredient in the recipe!", "Recipe Mechanic");
            return;
        }
        Recipe = new RecipeDataModel
        {
            Id = item.Id,
            OutputQuantity = (int)OutputQuantityInput.Value,
            CraftingDuration = (int)CraftingDurationInput.Value,
            Ingredients = IngredientList.Items.OfType<IngredientItemModel>().Select(ingredientItem => new IngredientDataModel { Id = ingredientItem.Id, Quantity = ingredientItem.Quantity }).ToList()
        };
        DialogResult = true;
        Close();
    }

}