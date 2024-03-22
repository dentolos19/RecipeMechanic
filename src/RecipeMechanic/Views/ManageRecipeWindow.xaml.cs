using System.Windows;
using RecipeMechanic.Core;
using RecipeMechanic.Models;

namespace RecipeMechanic.Views;

public partial class ManageRecipeWindow
{
    private readonly GameItemModel[] _items;
    private readonly RecipeDataModel[]? _recipes;

    private bool _massEditing;

    public RecipeDataModel? Recipe { get; private set; }

    public ManageRecipeWindow(GameItemModel[] items, RecipeDataModel[]? recipes = null)
    {
        _items = items;
        _recipes = recipes;
        InitializeComponent();
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        foreach (var item in _items)
            OutputItemSelection.Items.Add(item);
        if (_recipes is { Length: 1 }) // Single recipe editing
        {
            var recipe = _recipes[0];
            OutputItemSelection.SelectedItem = _items.FirstOrDefault(item => item.Id.Equals(recipe.Id));
            OutputQuantityInput.Value = recipe.OutputQuantity;
            CraftingDurationInput.Value = recipe.CraftingDuration;
            foreach (var ingredient in recipe.Ingredients)
                IngredientList.Items.Add(Utilities.ConvertToIngredientItem(ingredient, _items));
        }
        else if (_recipes is { Length: > 1 }) // Multi recipe editing
        {
            var hasSameQuantity = true;
            var hasSameDuration = true;
            var hasSameIngredients = true;
            for (var index = 1; index < _recipes.Length; index++)
            {
                if (_recipes[index - 1].OutputQuantity == _recipes[index].OutputQuantity)
                    continue;
                hasSameQuantity = false;
                break;
            }
            for (var index = 1; index < _recipes.Length; index++)
            {
                if (_recipes[index - 1].CraftingDuration == _recipes[index].CraftingDuration)
                    continue;
                hasSameDuration = false;
                break;
            }
            for (var index = 1; index < _recipes.Length; index++)
            {
                if (_recipes[index - 1].Ingredients.All(_recipes[index].Ingredients.Contains)
                    && _recipes[index - 1].Ingredients.Count == _recipes[index].Ingredients.Count)
                    continue;
                hasSameIngredients = false;
                break;
            }
            if (hasSameQuantity)
                OutputQuantityInput.Value = _recipes[0].OutputQuantity;
            if (hasSameDuration)
                CraftingDurationInput.Value = _recipes[0].CraftingDuration;
            if (hasSameIngredients)
                foreach (var ingredient in _recipes[0].Ingredients)
                    IngredientList.Items.Add(Utilities.ConvertToIngredientItem(ingredient, _items));
            OutputItemSelection.IsEnabled = false;
        }
        else // New recipe editing
        {
            var random = new Random();
            OutputItemSelection.SelectedIndex = random.Next(OutputItemSelection.Items.Count - 1);
            OutputQuantityInput.Value = OutputQuantityInput.Minimum;
            CraftingDurationInput.Value = CraftingDurationInput.Minimum;
            IngredientList.Items.Add(Utilities.ConvertToIngredientItem(new IngredientDataModel { Id = _items[random.Next(_items.Length - 1)].Id }, _items));
        }
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
        if (_recipes is { Length: > 1 }) // Multi recipe editing
        {
            var recipe = new RecipeDataModel();
            if (OutputQuantityInput.Value.HasValue)
                recipe.OutputQuantity = OutputQuantityInput.Value;
            if (CraftingDurationInput.Value.HasValue)
                recipe.CraftingDuration = CraftingDurationInput.Value;
            if (IngredientList.Items.Count > 0)
                recipe.Ingredients = IngredientList.Items.OfType<IngredientItemModel>().Select(ingredientItem => new IngredientDataModel { Id = ingredientItem.Id, Quantity = ingredientItem.Quantity }).ToList();
            Recipe = recipe;
        }
        else // New and single recipe editing
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
        }
        DialogResult = true;
        Close();
    }
}