using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using RecipeMechanic.Core;
using RecipeMechanic.Models;

namespace RecipeMechanic.Views;

public partial class MainWindow
{
    private string? _gamePath;
    private string? _recipePath;
    private GameItemModel[]? _items;

    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
        ((CollectionView)CollectionViewSource.GetDefaultView(RecipeList.ItemsSource)).Filter = FilterRecipe;
        ((CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource)).Filter = FilterItem;
    }

    private void UpdateStatus()
    {
        ViewModel.ItemCountText = $"{ViewModel.Items.Count} Item(s)";
        ViewModel.RecipeCountText = $"{ViewModel.Recipes.Count} Recipe(s)";
        if (!string.IsNullOrEmpty(_recipePath))
            ViewModel.OpenedFileText = _recipePath;
    }

    private void SaveRecipe(string filePath)
    {
        var recipes = ViewModel.Recipes.Select(item => item.Data).ToArray();
        var recipeData = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, "// This file was edited by Recipe Mechanic" + Environment.NewLine + recipeData);
    }

    private bool FilterRecipe(object item)
    {
        var filter = RecipeSearchInput.Text;
        if (string.IsNullOrEmpty(filter))
            return true; // does not filter item (keeps it)
        if (item is not RecipeItemModel recipeItem)
            return false; // filter item (hides it)
        return recipeItem.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase)
               || recipeItem.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
               || recipeItem.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true;
    }

    private bool FilterItem(object item)
    {
        var filter = ItemSearchInput.Text;
        if (string.IsNullOrEmpty(filter))
            return true; // does not filter item (keeps it)
        if (item is not GameItemModel gameitem)
            return false; // filter item (hides it)
        return gameitem.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase)
               || gameitem.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
               || gameitem.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true;
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        ViewModel.AppVersionText = $"v{version.Major}.{version.Minor}";
        UpdateStatus();
    }

    private void OnOpenRecipe(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenRecipeFileWindow { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        _gamePath = dialog.GamePath;
        _recipePath = dialog.RecipePath;
        try
        {
            if (_items is null)
            {
                var survivalDescriptions = Utilities.GetDescriptions(Path.Combine(_gamePath, "Survival/Gui/Language/English/inventoryDescriptions.json"));
                var descriptions = Utilities.GetDescriptions(Path.Combine(_gamePath, "Data/Gui/Language/English/InventoryItemDescriptions.json"));
                var mergedDescriptions = descriptions.Concat(survivalDescriptions.Where(pair => !descriptions.ContainsKey(pair.Key))).ToDictionary(pair => pair.Key, pair => pair.Value);
                var items = new List<GameItemModel>();
                foreach (var (id, description) in mergedDescriptions)
                    items.Add(new GameItemModel
                    {
                        Id = id,
                        Name = description.Title,
                        Description = description.Description
                    });
                _items = items.OrderBy(item => item.Name).ToArray();
                ViewModel.Items.Clear();
                foreach (var item in _items)
                    ViewModel.Items.Add(item);
            }
            var recipes = Utilities.GetRecipes(_recipePath);
            ViewModel.Recipes.Clear();
            foreach (var recipeItem in Utilities.MergeRecipesAndDescriptions(recipes, _items).OrderBy(item => item.Name))
                ViewModel.Recipes.Add(recipeItem);
        }
        catch (Exception exception)
        {
            _gamePath = null;
            _recipePath = null;
            ViewModel.Recipes.Clear();
            MessageBox.Show("An unhandled exception occurred while opening the recipe file! " + exception.Message, "Recipe Mechanic");
        }
        UpdateStatus();
    }

    private void OnSaveRecipe(object sender, RoutedEventArgs args)
    {
        if (string.IsNullOrEmpty(_recipePath))
        {
            OnSaveRecipeAs(null, null);
            return;
        }
        SaveRecipe(_recipePath);
    }

    private void OnSaveRecipeAs(object sender, RoutedEventArgs args)
    {
        if (!(ViewModel.Recipes.Count > 0))
        {
            MessageBox.Show("You must at least have one recipe in order to perform this task!", "Recipe Mechanic");
            return;
        }
        var dialog = new SaveFileDialog { Filter = "Recipe File (*.json)|*.json" };
        if (dialog.ShowDialog() != true)
            return;
        _recipePath = dialog.FileName;
        SaveRecipe(_recipePath);
        UpdateStatus();
    }

    private void OnExit(object sender, RoutedEventArgs args)
    {
        Application.Current.Shutdown();
    }

    private void OnRecipeSearch(object sender, TextChangedEventArgs args)
    {
        CollectionViewSource.GetDefaultView(RecipeList.ItemsSource).Refresh();
    }

    private void OnItemSearch(object sender, TextChangedEventArgs args)
    {
        CollectionViewSource.GetDefaultView(ItemList.ItemsSource).Refresh();
    }

    private void OnAddRecipe(object sender, RoutedEventArgs args)
    {
        if (_items is not { Length: > 0 })
        {
            MessageBox.Show("Open a recipe file first before performing this task!", "Recipe Mechanic");
            return;
        }
        var dialog = new ManageRecipeWindow(_items) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        var recipeItem = Utilities.ConvertToRecipeItem(dialog.Recipe, _items);
        var existingRecipeItem = RecipeList.Items.OfType<RecipeItemModel>().FirstOrDefault(item => item.Id.Equals(recipeItem.Id));
        if (existingRecipeItem is not null)
        {
            if (MessageBox.Show("The recipe already exists, do you want to replace it?", "Recipe Mechanic", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            ViewModel.Recipes.Remove(existingRecipeItem);
        }
        ViewModel.Recipes.Add(recipeItem);
        RecipeList.SelectedItem = recipeItem;
        UpdateStatus();
    }

    private void OnEditRecipes(object sender, RoutedEventArgs args)
    {
        if (!(RecipeList.SelectedItems.Count > 0))
            return;
        var items = RecipeList.SelectedItems.OfType<RecipeItemModel>().ToArray();
        var dialog = new ManageRecipeWindow(_items, items.Select(item => item.Data).ToArray()) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        foreach (var item in items)
        {
            var recipe = item.Data;
            if (dialog.Recipe.Id.HasValue)
                recipe.Id = dialog.Recipe.Id;
            if (dialog.Recipe.OutputQuantity.HasValue)
                recipe.OutputQuantity = dialog.Recipe.OutputQuantity;
            if (dialog.Recipe.CraftingDuration.HasValue)
                recipe.CraftingDuration = dialog.Recipe.CraftingDuration;
            if (dialog.Recipe.Ingredients is { Count: > 0 })
                recipe.Ingredients = dialog.Recipe.Ingredients;
            ViewModel.Recipes[ViewModel.Recipes.IndexOf(item)] = Utilities.ConvertToRecipeItem(recipe, _items);
        }
    }

    private void OnEditAllRecipes(object sender, RoutedEventArgs args)
    {
        if (_items is not { Length: > 0 })
        {
            MessageBox.Show("Open a recipe file first before performing this task!", "Recipe Mechanic");
            return;
        }
        var items = ViewModel.Recipes.ToArray();
        var dialog = new ManageRecipeWindow(_items, items.Select(item => item.Data).ToArray()) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        foreach (var item in items)
        {
            var recipe = item.Data;
            if (dialog.Recipe.Id.HasValue)
                recipe.Id = dialog.Recipe.Id;
            if (dialog.Recipe.OutputQuantity.HasValue)
                recipe.OutputQuantity = dialog.Recipe.OutputQuantity;
            if (dialog.Recipe.CraftingDuration.HasValue)
                recipe.CraftingDuration = dialog.Recipe.CraftingDuration;
            if (dialog.Recipe.Ingredients is { Count: > 0 })
                recipe.Ingredients = dialog.Recipe.Ingredients;
            ViewModel.Recipes[ViewModel.Recipes.IndexOf(item)] = Utilities.ConvertToRecipeItem(recipe, _items);
        }
    }

    private void OnRemoveRecipes(object sender, RoutedEventArgs args)
    {
        if (!(RecipeList.SelectedItems.Count > 0))
            return;
        if (MessageBox.Show("Are you sure that you want to remove these recipe(s)?", "Recipe Mechanic", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        foreach (var item in RecipeList.SelectedItems.OfType<RecipeItemModel>().ToArray())
            ViewModel.Recipes.Remove(item);
        UpdateStatus();
    }

    private void OnCopyRecipeId(object sender, RoutedEventArgs args)
    {
        if (RecipeList.SelectedItem is RecipeItemModel item)
            Clipboard.SetText(item.Id.ToString());
    }

    private void OnCopyItemId(object sender, RoutedEventArgs args)
    {
        if (ItemList.SelectedItem is GameItemModel item)
            Clipboard.SetText(item.Id.ToString());
    }

    private void OnLaunchBackupWizard(object sender, RoutedEventArgs args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "steam://backup/387990",
            UseShellExecute = true
        });
    }

    private void OnVerifyGameFiles(object sender, RoutedEventArgs args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "steam://validate/387990",
            UseShellExecute = true
        });
    }
}