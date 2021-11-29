using RecipeMechanic.Core;
using RecipeMechanic.Models;
using System;
using System.Linq;
using System.Windows;

namespace RecipeMechanic.Views;

public partial class ManageRecipeWindow
{

    private GameItemModel[] _items;

    public RecipeDataModel? Recipe { get; private set; }

    public ManageRecipeWindow(GameItemModel[] items, RecipeDataModel? recipe = null)
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
                IngredientList.Items.Add(Utilities.ConvertToIngredientItem(ingredient, _items));
        }
        var random = new Random();
        if (OutputItemSelection.SelectedItem is null)
            OutputItemSelection.SelectedIndex = random.Next(OutputItemSelection.Items.Count - 1);
        if (!(IngredientList.Items.Count > 0))
            IngredientList.Items.Add(Utilities.ConvertToIngredientItem(new IngredientDataModel { Id = _items[random.Next(_items.Length - 1)].Id }, _items));
    }

    private void OnAddIngredient(object sender, RoutedEventArgs args)
    {
        var dialog = new ManageIngredientWindow(_items) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        var ingredientItem = Utilities.ConvertToIngredientItem(dialog.Ingredient, _items);
        var existingIngredientItem = IngredientList.Items.OfType<IngredientItemModel>().FirstOrDefault(item => item.Id.Equals(ingredientItem.Id));
        if (existingIngredientItem is not null)
        {
            if (MessageBox.Show("The ingredient already exists in the recipe, do you want to replace it?", "Recipe Mechanic", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            IngredientList.Items.Remove(existingIngredientItem);
        }
        IngredientList.Items.Add(ingredientItem);
        IngredientList.SelectedItem = ingredientItem;
    }

    private void OnModifyIngredient(object sender, RoutedEventArgs args)
    {
        if (IngredientList.SelectedItem is not IngredientItemModel item)
            return;
        var dialog = new ManageIngredientWindow(_items, item.Data) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        IngredientList.Items.Remove(item);
        var ingredientItem = Utilities.ConvertToIngredientItem(dialog.Ingredient, _items);
        IngredientList.Items.Add(ingredientItem);
        IngredientList.SelectedItem = ingredientItem;
    }

    private void OnRemoveIngredient(object sender, RoutedEventArgs args)
    {
        if (IngredientList.SelectedItem is not IngredientItemModel item)
            return;
        if (MessageBox.Show("Are you sure that you want to delete this ingredient?", "Recipe Mechanic", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            IngredientList.Items.Remove(item);
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