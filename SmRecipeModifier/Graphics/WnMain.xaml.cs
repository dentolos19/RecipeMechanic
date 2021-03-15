using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnMain
    {

        private string _selectedPath;

        public WnMain()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(App.Settings.GameDataPath))
            {
                var dialog = new WnIntro();
                if (dialog.ShowDialog() == false)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            App.AvailableItems = Utilities.GetItemsFromJsons(Path.Combine(App.Settings.GameDataPath!, Constants.InventoryItemDescriptionsJson), Path.Combine(App.Settings.GameDataPath, Constants.InventoryDescriptionsJson));
            foreach (var item in App.AvailableItems)
                ItemList.Items.Add(item);
            ItemListItemAmountText.Text = $"There are a total of {App.AvailableItems.Length} items in-game!";
        }

        private void Open(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _selectedPath = dialog.SelectedPath;
                FileNameBox.Text = Path.GetFileName(_selectedPath)!;
                var recipes = Utilities.GetRecipesFromJson(_selectedPath);
                var items = Utilities.MergeRecipesWithItems(recipes, App.AvailableItems);
                RecipeList.Items.Clear();
                foreach (var item in items)
                    RecipeList.Items.Add(item);
                RecipeListItemAmountText.Text = $"There are a total of {items.Length} items in this file!";
                SaveButton.IsEnabled = true;
                SaveAsButton.IsEnabled = true;
                SaveMenuButton.IsEnabled = true;
                SaveAsMenuButton.IsEnabled = true;
                AddRecipeButton.IsEnabled = true;
                MassModificationMenu.IsEnabled = true;
            }
        }

        private void SaveToFile(string path)
        {
            var items = RecipeList.Items.OfType<SmItem>();
            var data = JsonConvert.SerializeObject(items.Select(item => item.Recipe).ToArray(), Formatting.Indented);
            File.WriteAllText(path, $"// This file was modified by SmRecipeModifier.\n{data}");
        }

        private void Save(object sender, ExecutedRoutedEventArgs args)
        {
            if (!SaveButton.IsEnabled)
                return;
            if (MessageBox.Show("Are you sure? This will overwrite the current recipe file in the game files.", "SmRecipeModifier", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                SaveToFile(_selectedPath);
        }

        private void SaveAs(object sender, ExecutedRoutedEventArgs args)
        {
            if (!SaveButton.IsEnabled)
                return;
            var dialog = new SaveFileDialog { Filter = "JSON Source File|*.json" };
            if (dialog.ShowDialog() == true)
                SaveToFile(dialog.FileName);
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void ShowPreferences(object sender, RoutedEventArgs args)
        {
            new WnPreferences { Owner = this }.ShowDialog();
        }

        private async void ShowAbout(object sender, RoutedEventArgs args)
        {
            await this.ShowMessageAsync("About SmRecipeModifier", "This program was created by Dennise Catolos.\n\nVersion: 1.1.1 (2021-03-XX)").ConfigureAwait(false);
        }

        private void AddRecipe(object sender, RoutedEventArgs args)
        {
            if (!AddRecipeButton.IsEnabled)
                return;
            var dialog = new WnNewRecipe { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            RecipeList.Items.Add(dialog.ItemResult);
            RecipeList.SelectedIndex = RecipeList.Items.Count - 1;
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            if (!RemoveRecipeButton.IsEnabled)
                return;
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            if (MessageBox.Show("Are you sure that you want to remove this recipe?", "SmRecipeModifier", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                RecipeList.Items.Remove(item);
        }

        private void ModifyRecipe(object sender, RoutedEventArgs args)
        {
            if (!ModifyRecipeButton.IsEnabled)
                return;
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            var dialog = new WnModifyRecipe(item.Recipe) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            RecipeList.Items.Remove(RecipeList.SelectedItem);
            item.Recipe = dialog.RecipeResult;
            RecipeList.Items.Add(item);
            RecipeList.SelectedIndex = RecipeList.Items.Count - 1;
        }

        private void ModifyAllRecipes(object sender, RoutedEventArgs args)
        {
            if (!MassModificationMenu.IsEnabled)
                return;
            var dialog = new WnModifyRecipe(null) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            var items = RecipeList.Items.OfType<SmItem>().ToArray();
            RecipeList.Items.Clear();
            foreach (var recipe in items)
            {
                recipe.Recipe.Quantity = dialog.RecipeResult.Quantity;
                recipe.Recipe.Duration = dialog.RecipeResult.Duration;
                recipe.Recipe.Requirements = dialog.RecipeResult.Requirements;
                RecipeList.Items.Add(recipe);
            }
        }

        private void CopyRecipeName(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Name);
        }
        
        private void CopyRecipeId(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
        }
        
        private void CopyRecipeDescription(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Description);
        }

        private void CopyItemName(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)ItemList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Name);
        }
        
        private void CopyItemId(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)ItemList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
        }
        
        private void CopyItemDescription(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)ItemList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Description);
        }

        private void OpenBackupWizard(object sender, RoutedEventArgs args)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "steam://backup/387990",
                UseShellExecute = true
            });
        }

        private void VerifyGameFiles(object sender, RoutedEventArgs args)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "steam://validate/387990",
                UseShellExecute = true
            });
        }

        private void UpdateRecipeListSelection(object sender, SelectionChangedEventArgs args)
        {
            if (RecipeList.SelectedItem == null)
            {
                RemoveRecipeButton.IsEnabled = false;
                ModifyRecipeButton.IsEnabled = false;
            }
            else
            {
                RemoveRecipeButton.IsEnabled = true;
                ModifyRecipeButton.IsEnabled = true;
            }
        }

        private void ActivateEasyMode(object sender, RoutedEventArgs args)
        {
            if (!MassModificationMenu.IsEnabled)
                return;
            MessageBox.Show("This function is currently disabled for now! Sorry for the inconvenience.", "SmRecipeModifier");
            // TODO
        }

        private void ActivateHardMode(object sender, RoutedEventArgs args)
        {
            if (!MassModificationMenu.IsEnabled)
                return;
            MessageBox.Show("This function is currently disabled for now! Sorry for the inconvenience.", "SmRecipeModifier");
            // TODO
        }
        
        private void NavigateHyperlink(object sender, RequestNavigateEventArgs args)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = args.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            args.Handled = true;
        }

    }

}