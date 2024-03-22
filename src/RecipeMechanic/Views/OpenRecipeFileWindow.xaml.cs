using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using RecipeMechanic.Models;

namespace RecipeMechanic.Views;

public partial class OpenRecipeFileWindow
{
    public string GamePath { get; private set; }
    public string RecipePath { get; private set; }

    public OpenRecipeFileWindow()
    {
        InitializeComponent();
    }

    private void LoadGameRecipes(string gamePath)
    {
        var recipesPath = Path.Combine(gamePath, "Survival", "CraftingRecipes");
        var cookBotRecipePath = Path.Combine(recipesPath, "cookbot.json");
        var craftBotRecipePath = Path.Combine(recipesPath, "craftbot.json");
        var dispenserRecipePath = Path.Combine(recipesPath, "dispenser.json");
        var hideoutRecipePath = Path.Combine(recipesPath, "hideout.json");
        var workbenchRecipePath = Path.Combine(recipesPath, "workbench.json");
        RecipeFileList.Items.Clear();
        if (File.Exists(cookBotRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel
            {
                Name = "cookbot.json",
                Description = "Your personal non-living chef!",
                Path = cookBotRecipePath
            });
        if (File.Exists(craftBotRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel
            {
                Name = "craftbot.json",
                Description = "The well-known Craftbot!",
                Path = craftBotRecipePath
            });
        if (File.Exists(dispenserRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel
            {
                Name = "dispenser.json",
                Description = "The one that creates the Craftbot!",
                Path = dispenserRecipePath
            });
        if (File.Exists(hideoutRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel
            {
                Name = "hideout.json",
                Description = "The person who sells things overpriced!",
                Path = hideoutRecipePath
            });
        if (File.Exists(workbenchRecipePath))
            RecipeFileList.Items.Add(new RecipeFileItemModel
            {
                Name = "workbench.json",
                Description = "The one that is at the starting ship!",
                Path = workbenchRecipePath
            });
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        if (string.IsNullOrEmpty(App.Settings.GamePath))
            return;
        GamePathInput.Text = App.Settings.GamePath;
        LoadGameRecipes(App.Settings.GamePath);
    }

    private void OnBrowseGame(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Game Executable (ScrapMechanic.exe)|ScrapMechanic.exe" };
        if (dialog.ShowDialog() != true)
            return;
        var gamePath = Path.GetDirectoryName(Path.GetDirectoryName(dialog.FileName));
        App.Settings.GamePath = gamePath;
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
            OnContinue(null!, null!);
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