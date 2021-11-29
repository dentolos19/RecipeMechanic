using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using RecipeMechanic.Core;
using RecipeMechanic.Models;
using RecipeMechanic.ViewModels;

namespace RecipeMechanic.Views;

public partial class MainWindow
{

    private string? _gamePath;
    private string? _recipePath;
    private GameItemModel[]? _gameItems;

    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
        ((CollectionView)CollectionViewSource.GetDefaultView(RecipeList.ItemsSource)).Filter = FilterRecipe;
    }

    private void SaveRecipe(string filePath)
    {
        var itemRecipes = ViewModel.RecipeList.Select(item => item.Data).ToArray();
        var recipeContent = JsonSerializer.Serialize(itemRecipes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, "// This file was edited by Recipe Mechanic" + Environment.NewLine + recipeContent);
    }

    private bool FilterRecipe(object item)
    {
        var filter = FilterInput.Text;
        if (string.IsNullOrEmpty(filter))
            return true; // does not filter item (keeps it)
        if (item is not RecipeItemModel recipeItem)
            return false; // filter item (hides it)
        return recipeItem.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
               recipeItem.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true ||
               recipeItem.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase);
    }

    private void OnOpenRecipe(object sender, ExecutedRoutedEventArgs args)
    {
        var dialog = new OpenRecipeFileWindow { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        _gamePath = dialog.GamePath;
        _recipePath = dialog.RecipePath;
        try
        {
            if (_gameItems is null)
            {
                var itemSurvivalDescriptions = Utilities.GetItemDescriptions(Path.Combine(_gamePath, "Survival/Gui/Language/English/inventoryDescriptions.json"));
                var itemDescriptions = Utilities.GetItemDescriptions(Path.Combine(_gamePath, "Data/Gui/Language/English/InventoryItemDescriptions.json"));
                var mergedItemDescriptions = itemDescriptions.Concat(itemSurvivalDescriptions.Where(pair => !itemDescriptions.ContainsKey(pair.Key))).ToDictionary(pair => pair.Key, pair => pair.Value);
                var items = new List<GameItemModel>();
                foreach (var (id, description) in mergedItemDescriptions)
                    items.Add(new GameItemModel
                    {
                        Id = id,
                        Name = description.Title,
                        Description = description.Description
                    });
                _gameItems = items.ToArray();
            }
            var itemRecipes = Utilities.GetItemRecipes(_recipePath);
            ViewModel.RecipeList.Clear();
            foreach (var recipeItem in Utilities.MergeRecipesAndDescriptions(itemRecipes, _gameItems))
                ViewModel.RecipeList.Add(recipeItem);
        }
        catch (Exception exception)
        {
            _gamePath = null;
            _recipePath = null;
            ViewModel.RecipeList.Clear();
            MessageBox.Show("An unhandled exception occurred while loading the recipe file! " + exception.Message, "Recipe Mechanic");
        }
    }

    private void CanSaveRecipe(object sender, CanExecuteRoutedEventArgs args)
    {
        args.CanExecute = !string.IsNullOrEmpty(_recipePath) && ViewModel.RecipeList.Count > 0;
    }

    private void OnSaveRecipe(object sender, ExecutedRoutedEventArgs args)
    {
        if (!string.IsNullOrEmpty(_recipePath))
            SaveRecipe(_recipePath);
    }

    private void CanSaveRecipeAs(object sender, CanExecuteRoutedEventArgs args)
    {
        args.CanExecute = ViewModel.RecipeList.Count > 0;
    }

    private void OnSaveRecipeAs(object sender, ExecutedRoutedEventArgs args)
    {
        var dialog = new SaveFileDialog { Filter = "Recipe File (*.json)|*.json" };
        if (dialog.ShowDialog() != true)
            return;
        _recipePath = dialog.FileName;
        SaveRecipe(_recipePath);
    }

    private void OnExit(object sender, RoutedEventArgs args)
    {
        Application.Current.Shutdown();
    }

    private void OnRecipeFilter(object sender, TextChangedEventArgs args)
    {
        CollectionViewSource.GetDefaultView(RecipeList.ItemsSource).Refresh();
    }

    private void IsItemsAvailable(object sender, CanExecuteRoutedEventArgs args)
    {
        args.CanExecute = _gameItems is { Length: > 0 };
    }

    private void IsRecipeSelected(object sender, CanExecuteRoutedEventArgs args)
    {
        args.CanExecute = RecipeList.SelectedItem is not null;
    }

    private void OnAddRecipe(object sender, ExecutedRoutedEventArgs args)
    {
        var dialog = new ModifyRecipeWindow(_gameItems) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        var recipeItem = Utilities.ConvertDataToItem(dialog.Recipe, _gameItems);
        ViewModel.RecipeList.Add(recipeItem);
        RecipeList.SelectedItem = recipeItem;
    }

    private void OnModifyRecipe(object sender, ExecutedRoutedEventArgs args)
    {
        if (RecipeList.SelectedItem is not RecipeItemModel item)
            return;
        var dialog = new ModifyRecipeWindow(_gameItems, item.Data) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        ViewModel.RecipeList.Remove(item);
        var recipeItem = Utilities.ConvertDataToItem(dialog.Recipe, _gameItems);
        ViewModel.RecipeList.Add(recipeItem);
        RecipeList.SelectedItem = recipeItem;
    }

    private void OnRemoveRecipe(object sender, ExecutedRoutedEventArgs args)
    {
        if (RecipeList.SelectedItem is not RecipeItemModel item)
            return;
        if (MessageBox.Show("Are you sure that you want to delete this recipe?", "Recipe Mechanic", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            ViewModel.RecipeList.Remove(item);
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