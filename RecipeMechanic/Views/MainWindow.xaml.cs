using Microsoft.Win32;
using RecipeMechanic.Core;
using RecipeMechanic.Models;
using RecipeMechanic.ViewModels;
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
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        ViewModel.AppVersionText = $"v{version.Major}.{version.Minor}";
        ((CollectionView)CollectionViewSource.GetDefaultView(RecipeList.ItemsSource)).Filter = FilterRecipe;
    }

    private void UpdateStatus()
    {
        ViewModel.RecipeCountText = $"{RecipeList.Items.Count} Recipe(s)";
        ViewModel.OpenedFilePath = _recipePath;
    }

    private void SaveRecipe(string filePath)
    {
        var recipes = ViewModel.RecipeList.Select(item => item.Data).ToArray();
        var recipeData = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, "// This file was edited by Recipe Mechanic" + Environment.NewLine + recipeData);
    }

    private bool FilterRecipe(object item)
    {
        var filter = SearchInput.Text;
        if (string.IsNullOrEmpty(filter))
            return true; // does not filter item (keeps it)
        if (item is not RecipeItemModel recipeItem)
            return false; // filter item (hides it)
        return recipeItem.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
               || recipeItem.Description?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true
               || recipeItem.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase);
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
                _items = items.ToArray();
            }
            var recipes = Utilities.GetRecipes(_recipePath);
            ViewModel.RecipeList.Clear();
            foreach (var recipeItem in Utilities.MergeRecipesAndDescriptions(recipes, _items))
                ViewModel.RecipeList.Add(recipeItem);
        }
        catch (Exception exception)
        {
            _gamePath = null;
            _recipePath = null;
            ViewModel.RecipeList.Clear();
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
        if (!(ViewModel.RecipeList.Count > 0))
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
        ViewModel.RecipeList.Add(recipeItem);
        RecipeList.SelectedItem = recipeItem;
        UpdateStatus();
    }

    private void OnModifyRecipe(object sender, RoutedEventArgs args)
    {
        if (RecipeList.SelectedItem is not RecipeItemModel item)
            return;
        var dialog = new ManageRecipeWindow(_items, item.Data) { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        ViewModel.RecipeList.Remove(item);
        var recipeItem = Utilities.ConvertToRecipeItem(dialog.Recipe, _items);
        ViewModel.RecipeList.Add(recipeItem);
        RecipeList.SelectedItem = recipeItem;
    }

    private void OnRemoveRecipe(object sender, RoutedEventArgs args)
    {
        if (RecipeList.SelectedItem is not RecipeItemModel item)
            return;
        if (MessageBox.Show("Are you sure that you want to delete this recipe?", "Recipe Mechanic", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        ViewModel.RecipeList.Remove(item);
        UpdateStatus();
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