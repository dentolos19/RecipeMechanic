using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
                var detectedPath = Utilities.DetectGameDataPath();
                if (detectedPath == null)
                {
                    var dialog = new WnPrerequisites();
                    if (dialog.ShowDialog() == false)
                    {
                        Application.Current.Shutdown();
                        return;
                    }   
                }
                else
                {
                    App.Settings.GameDataPath = detectedPath;
                    App.Settings.Save();
                }
            }
            App.AvailableItems = Utilities.GetItemsFromJsons(Path.Combine(App.Settings.GameDataPath!, Constants.GameInventoryItemDescriptionsJson), Path.Combine(App.Settings.GameDataPath, Constants.SurvivalInventoryDescriptionsJson)).ToList();
            ItemList.ItemsSource = App.AvailableItems;
            ((CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource)).Filter = FilterItems;
            ItemListItemAmountText.Text = $"There are a total of {App.AvailableItems.Count} items in-game!";
        }
        
        private void SaveToFile(string path)
        {
            var items = RecipeList.Items.OfType<SmItem>();
            var data = JsonConvert.SerializeObject(items.Select(item => item.Recipe).ToArray(), Formatting.Indented);
            File.WriteAllText(path, $"// This file was modified by SmRecipeModifier.\n{data}");
        }

        private bool FilterRecipes(object query)
        {
            if (string.IsNullOrEmpty(RecipeListFilterInput.Text))
                return true;
            var item = (SmItem)query;
            var nameMatched = false;
            if (App.Settings.EnableNameFilter)
                nameMatched = item.Name.Contains(RecipeListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            var idMatched = false;
            if (App.Settings.EnableIdFilter)
                idMatched = item.Id.Contains(RecipeListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            var descriptionMatched = false;
            if (App.Settings.EnableDescriptionFilter && !string.IsNullOrEmpty(item.Description))
                descriptionMatched = item.Description.Contains(RecipeListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            return nameMatched || idMatched || descriptionMatched;
        }

        private bool FilterItems(object query)
        {
            if (string.IsNullOrEmpty(ItemListFilterInput.Text))
                return true;
            var item = (SmItem)query;
            var nameMatched = false;
            if (App.Settings.EnableNameFilter)
                nameMatched = item.Name.Contains(ItemListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            var idMatched = false;
            if (App.Settings.EnableIdFilter)
                idMatched = item.Id.Contains(ItemListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            var descriptionMatched = false;
            if (App.Settings.EnableDescriptionFilter && !string.IsNullOrEmpty(item.Description))
                descriptionMatched = item.Description.Contains(ItemListFilterInput.Text, StringComparison.OrdinalIgnoreCase);
            return nameMatched || idMatched || descriptionMatched;
        }

        private void Open(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new WnOpenRecipe { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _selectedPath = dialog.SelectedPath;
                FileNameBox.Text = Path.GetFileName(_selectedPath)!;
                var recipes = Utilities.GetRecipesFromJson(_selectedPath);
                App.RecipeItems = Utilities.MergeRecipesWithItems(recipes, App.AvailableItems.ToArray()).ToList();
                RecipeList.ItemsSource = App.RecipeItems;
                ((CollectionView)CollectionViewSource.GetDefaultView(RecipeList.ItemsSource)).Filter = FilterRecipes;
                RecipeListItemAmountText.Text = $"There are a total of {App.RecipeItems.Count} recipes in this file!";
                SaveButton.IsEnabled = true;
                SaveAsButton.IsEnabled = true;
                SaveMenuButton.IsEnabled = true;
                SaveAsMenuButton.IsEnabled = true;
                AddRecipeButton.IsEnabled = true;
                MassModificationMenu.IsEnabled = true;
                RecipeListFilterInput.IsEnabled = true;
            }
        }

        private void Save(object sender, ExecutedRoutedEventArgs args)
        {
            if (!SaveButton.IsEnabled)
                return;
            if (MessageBox.Show("Are you sure? This will overwrite the current recipe file in the game files.", Application.Current.Resources["String_DialogWinTitle"].ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

        private void Restart(object sender, RoutedEventArgs args)
        {
            Utilities.RestartApp();
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
            await this.ShowMessageAsync(Application.Current.Resources["String_AboutTitle"].ToString(), Application.Current.Resources["String_AboutText"].ToString()).ConfigureAwait(false);
        }

        private void AddRecipe(object sender, RoutedEventArgs args)
        {
            if (!AddRecipeButton.IsEnabled)
                return;
            var dialog = new WnNewRecipe { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            App.RecipeItems.Add(dialog.ItemResult);
            RefreshRecipeList(null, null);
            RecipeList.SelectedIndex = App.RecipeItems.IndexOf(dialog.ItemResult);
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            if (!RemoveRecipeButton.IsEnabled)
                return;
            if (MessageBox.Show("Are you sure that you want to remove this recipe(s)?", Application.Current.Resources["String_DialogWinTitle"].ToString(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            foreach (var selectedItem in RecipeList.SelectedItems.Cast<SmItem>().ToArray())
                App.RecipeItems.Remove(selectedItem);
            RefreshRecipeList(null, null);
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
            App.RecipeItems.First(recipe => recipe == item).Recipe = dialog.RecipeResult;
            RefreshRecipeList(null, null);
        }
        
        private void ModifyAllRecipes(object sender, RoutedEventArgs args)
        {
            if (!MassModificationMenu.IsEnabled)
                return;
            var dialog = new WnModifyRecipe { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            App.RecipeItems.ForEach(recipe =>
            {
                recipe.Recipe.Quantity = dialog.RecipeResult.Quantity;
                recipe.Recipe.Duration = dialog.RecipeResult.Duration;
                recipe.Recipe.Requirements = dialog.RecipeResult.Requirements;
            });
            RefreshRecipeList(null, null);
        }

        private void CopyRecipeName(object sender, RoutedEventArgs args)
        {
            if (RecipeList.SelectedItem is SmItem item)
                Clipboard.SetText(item.Name);
        }
        
        private void CopyRecipeId(object sender, RoutedEventArgs args)
        {
            if (RecipeList.SelectedItem is SmItem item)
                Clipboard.SetText(item.Id);
        }
        
        private void CopyRecipeDescription(object sender, RoutedEventArgs args)
        {
            if (RecipeList.SelectedItem is SmItem item)
                Clipboard.SetText(item.Description);
        }

        private void CopyItemName(object sender, RoutedEventArgs args)
        {
            if (ItemList.SelectedItem is SmItem item)
                Clipboard.SetText(item.Name);
        }
        
        private void CopyItemId(object sender, RoutedEventArgs args)
        {
            if (ItemList.SelectedItem is SmItem item)
                Clipboard.SetText(item.Id);
        }
        
        private void CopyItemDescription(object sender, RoutedEventArgs args)
        {
            if (ItemList.SelectedItem is SmItem item)
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

        private void NavigateHyperlink(object sender, RequestNavigateEventArgs args)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = args.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            args.Handled = true;
        }

        private void RefreshRecipeList(object sender, TextChangedEventArgs args)
        {
            CollectionViewSource.GetDefaultView(RecipeList.ItemsSource).Refresh();
        }

        private void RefreshItemList(object sender, TextChangedEventArgs args)
        {
            CollectionViewSource.GetDefaultView(ItemList.ItemsSource).Refresh();
        }

    }

}