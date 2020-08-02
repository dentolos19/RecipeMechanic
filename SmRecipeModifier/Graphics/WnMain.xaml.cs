﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private readonly SmItem[] _availableItems;

        public WnMain()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(App.Settings.GameDataPath))
                new WnIntro().ShowDialog();
            _availableItems = Utilities.GetItemsFromJsons(Path.Combine(App.Settings.GameDataPath!, Constants.ItemNamesJson), Path.Combine(App.Settings.GameDataPath, Constants.InventoryDescriptionsJson));
            foreach (var item in _availableItems)
                ItemList.Items.Add(item);
            ItemListItemAmountText.Text = $"There are a total of {_availableItems.Length} survival items in-game!";
        }

        private void Open(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _selectedPath = dialog.SelectedPath;
                FileNameBox.Text = Path.GetFileName(_selectedPath)!;
                var recipes = Utilities.GetRecipesFromJson(_selectedPath);
                var items = Utilities.MergeRecipesWithItems(recipes, _availableItems);
                RecipeList.Items.Clear();
                foreach (var item in items)
                    RecipeList.Items.Add(item);
                RecipeListItemAmountText.Text = $"There are a total of {items.Length} survival items in this file!";
                SaveButton.IsEnabled = true;
                SaveAsButton.IsEnabled = true;
                SaveMenuButton.IsEnabled = true;
                SaveAsMenuButton.IsEnabled = true;
                AddRecipeButton.IsEnabled = true;
            }
        }

        private void SaveToFile(string path)
        {
            var items = RecipeList.Items.OfType<SmItem>();
            var data = JsonConvert.SerializeObject(items.Select(item => item.Recipe).ToArray(), Formatting.Indented);
            File.WriteAllText($"// This file was created by SmRecipeModifier.\n{data}", data);
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
            await this.ShowMessageAsync("About SmRecipeModifier", "This program was created by Dennise Catolos.\n\nContact me on Discord, @dentolos19#6996.\nFind me on GitHub, @dentolos19.\nFind me on Twitter, @dentolos19.");
        }

        private void AddRecipe(object sender, RoutedEventArgs args)
        {
            if (!AddRecipeButton.IsEnabled)
                return;
            // TODO
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
            // TODO
        }

        private void CopyRecipeId(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)RecipeList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
        }

        private void CopyItemId(object sender, RoutedEventArgs args)
        {
            var item = (SmItem)ItemList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
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

    }

}