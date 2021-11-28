using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using RecipeMechanic.Models;

namespace RecipeMechanic.Views;

public partial class OpenRecipeWindow
{

    public string GamePath { get; private set; }
    public string RecipePath { get; private set; }

    public OpenRecipeWindow()
    {
        InitializeComponent();
    }

    private void LoadGameRecipes(string gamePath)
    {
        var cookBotRecipePath = Path.Combine(gamePath, "Survival/CraftingRecipes/cookbot.json");
        var craftBotRecipePath = Path.Combine(gamePath, "Survival/CraftingRecipes/craftbot.json");
        var dispenserRecipePath = Path.Combine(gamePath, "Survival/CraftingRecipes/dispenser.json");
        var hideoutRecipePath = Path.Combine(gamePath, "Survival/CraftingRecipes/hideout.json");
        var workbenchRecipePath = Path.Combine(gamePath,"Survival/CraftingRecipes/workbench.json");
        RecipeFileList.Items.Clear();
        if (File.Exists(cookBotRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel("cookbot.json", "Your personal non-living chef!", cookBotRecipePath));
        if (File.Exists(craftBotRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel("craftbot.json", "The well-known Craftbot!", craftBotRecipePath));
        if (File.Exists(dispenserRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel("dispenser.json", "The one that creates the Craftbot!", dispenserRecipePath));
        if (File.Exists(hideoutRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel("hideout.json", "The person who sells things overpriced!", hideoutRecipePath));
        if (File.Exists(workbenchRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel("workbench.json", "The one that is at the starting ship!", workbenchRecipePath));
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        if (string.IsNullOrEmpty(App.Settings.SavedGamePath))
            return;
        GamePathInput.Text = App.Settings.SavedGamePath;
        LoadGameRecipes(App.Settings.SavedGamePath);
    }

    private void OnBrowseGame(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Game Executable (ScrapMechanic.exe)|ScrapMechanic.exe" };
        if (dialog.ShowDialog() != true)
            return;
        var gamePath = Path.GetDirectoryName(Path.GetDirectoryName(dialog.FileName));
        App.Settings.SavedGamePath = gamePath;
        GamePathInput.Text = gamePath;
        LoadGameRecipes(gamePath);
    }

    private void OnBrowseRecipe(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Recipe File (*.json)|*.json" };
        if (dialog.ShowDialog() != true)
            return;
        RecipePathInput.Text = dialog.FileName;
    }

    private void OnItemSelect(object sender, SelectionChangedEventArgs args)
    {
        if (RecipeFileList.SelectedItem is RecipeFileItemModel item)
            RecipePathInput.Text = item.Path;
    }

    private void OnItemSelected(object sender, MouseButtonEventArgs args)
    {
        if (RecipeFileList.SelectedItem is RecipeFileItemModel item)
            OnContinue(null, null);
    }

    private void OnContinue(object sender, RoutedEventArgs args)
    {
        var gamePath = GamePathInput.Text;
        var recipePath = RecipePathInput.Text;
        if (string.IsNullOrEmpty(gamePath))
        {
            MessageBox.Show("Select the game path before continuing!", "Recipe Mechanic");
            return;
        }
        if (string.IsNullOrEmpty(recipePath))
        {
            MessageBox.Show("Select a recipe file before continuing!", "Recipe Mechanic");
            return;
        }
        GamePath = gamePath;
        RecipePath = recipePath;
        DialogResult = true;
        Close();
    }

}