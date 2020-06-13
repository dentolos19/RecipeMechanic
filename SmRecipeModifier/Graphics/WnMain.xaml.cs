using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnMain
    {

        private string _path;
        public SmItemInfoDictionary InfoDictionary;
        public SmItemDictionary ItemDictionary;

        public WnMain()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(App.Settings.GameDataPath))
                return;
            MiCreateBackup.IsEnabled = true;
            MiApplyBackup.IsEnabled = true;
        }

        private void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _path = dialog.Result;
            TbFileName.Text = Path.GetFileName(_path)!;
            BnSave.IsEnabled = true;
            BnSaveAs.IsEnabled = true;
            MiSave.IsEnabled = true;
            MiSaveAs.IsEnabled = true;
            BnAddRecipe.IsEnabled = true;
            MiCreateBackup.IsEnabled = true;
            MiApplyBackup.IsEnabled = true;
            MiEditAllData.IsEnabled = true;
            MiActivateEasyMode.IsEnabled = true;
            LvRecipes.Items.Clear();
            InfoDictionary = new SmItemInfoDictionary(Path.Combine(App.Settings.GameDataPath, Constants.ItemDescriptionsJsonPath));
            ItemDictionary = new SmItemDictionary(Path.Combine(App.Settings.GameDataPath, Constants.ItemNamesJsonPath));
            var recipes = new SmRecipeDictionary(_path).Items;
            foreach (var recipe in recipes)
                AddRecipeToList(recipe);
        }

        private void AddRecipeToList(SmRecipe recipe)
        {
            var info = InfoDictionary.Fetch(recipe.Id);
            if (info != null)
            {
                LvRecipes.Items.Add(new LvRecipeBinding(info.Info.Title, info.Id, recipe));
            }
            else
            {
                info = ItemDictionary.Fetch(recipe.Id);
                if (info != null)
                    LvRecipes.Items.Add(new LvRecipeBinding(info.Name, info.Id, recipe));
            }
        }

        private void Save(object sender, RoutedEventArgs args)
        {
            if (BnSave.IsEnabled == false)
                return;
            if (!File.Exists(Path.Combine(App.Settings.GameDataPath, "SmRecipeModifier.bak")))
                if (MessageBox.Show("You haven't set a backup yet! Do you want to continue?", "I want to ensure your safety!", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;
            if (MessageBox.Show("Are you sure that you want to apply all recipe changes?", "Think twice about it!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                SaveToFile(_path);
        }

        private void SaveAs(object sender, RoutedEventArgs args)
        {
            if (BnSave.IsEnabled == false)
                return;
            var dialog = new SaveFileDialog { Filter = "Json File|*.json" };
            if (dialog.ShowDialog() == false)
                return;
            SaveToFile(dialog.FileName);
        }

        private void SaveToFile(string path)
        {
            var list = new List<SmRecipe>();
            foreach (var item in LvRecipes.Items.OfType<LvRecipeBinding>())
                list.Add(item.Recipe);
            var data = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, $"// This file is modified by SmRecipeModifier (https://dennise.me/projects/smrecipemodifier)\n{data}");
        }

        private void AddRecipe(object sender, RoutedEventArgs args)
        {
            MessageBox.Show("This function is not available yet.", "Sorry about that!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            if (LvRecipes.SelectedItem == null)
                return;
            var item = LvRecipes.SelectedItem as LvRecipeBinding;
            RemoveSpecificRecipe(item?.Recipe);
        }

        private void RemoveSpecificRecipe(SmRecipe recipe)
        {
            foreach (var item in LvRecipes.Items.OfType<LvRecipeBinding>())
                if (item.Recipe.Id == recipe.Id)
                {
                    if (MessageBox.Show("Are you sure? Make sure you know what you are doing.", "This might break the game!", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                        return;
                    LvRecipes.Items.Remove(item);
                    break;
                }
        }

        private void ModifyRecipe(object sender, RoutedEventArgs args)
        {
            if (LvRecipes.SelectedItem == null)
                return;
            var item = LvRecipes.SelectedItem as LvRecipeBinding;
            var dialog = new WnModify(item?.Recipe) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            RemoveSpecificRecipe(item?.Recipe);
            AddRecipeToList(dialog.Result);
        }

        private void RecipeSelectionUpdate(object sender, SelectionChangedEventArgs args)
        {
            if (LvRecipes.SelectedItem == null)
            {
                BnRemoveRecipe.IsEnabled = false;
                BnModifyRecipe.IsEnabled = false;
            }
            else
            {
                BnRemoveRecipe.IsEnabled = true;
                BnModifyRecipe.IsEnabled = true;
            }
        }

        private void CreateBackup(object sender, RoutedEventArgs args)
        {
            var backupPath = Path.Combine(App.Settings.GameDataPath, "SmRecipeModifier.bak");
            if (File.Exists(backupPath))
            {
                MessageBox.Show("Backup file is already created so you don't need to do it again.", "It already there!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            ZipFile.CreateFromDirectory(Path.Combine(App.Settings.GameDataPath, "Survival"), backupPath);
            MessageBox.Show("Backup file created successfully.", "I ensure your safety!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyBackup(object sender, RoutedEventArgs args)
        {
            var backupPath = Path.Combine(App.Settings.GameDataPath, "SmRecipeModifier.bak");
            if (!File.Exists(backupPath))
            {
                MessageBox.Show("Backup does not exist! Try using file verification via steam.", "Oops! No backup file found.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure that you want to apply backup?", "Just want to make sure!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            ZipFile.ExtractToDirectory(backupPath, Path.Combine(App.Settings.GameDataPath, "Survival"), true);
            MessageBox.Show("Applied backup successfully!", "Safety ensured!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditAllData(object sender, RoutedEventArgs args)
        {
            if (MessageBox.Show("Are you sure? This will affect all recipes.", "Mass murdering numbers!", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            var dialog = new WnModify(null, true) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            var list = LvRecipes.Items.OfType<LvRecipeBinding>().ToArray();
            LvRecipes.Items.Clear();
            foreach (var item in list)
            {
                var newRecipe = item.Recipe;
                newRecipe.Quantity = dialog.Result.Quantity;
                newRecipe.Duration = dialog.Result.Duration;
                newRecipe.Requirements = item.Recipe.Requirements;
                AddRecipeToList(newRecipe);
            }
            MessageBox.Show("Successfully replaced every recipe in this json file.", "Activated noobie mode!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ActivateEasyMode(object sender, RoutedEventArgs args)
        {
            if (MessageBox.Show("Are you sure? This will allow you to craft anything without grinding.", "This is quite noobish!", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            var list = LvRecipes.Items.OfType<LvRecipeBinding>().ToArray();
            LvRecipes.Items.Clear();
            foreach (var item in list)
            {
                var newRecipe = item.Recipe;
                var newRequirements = new List<SmRequirement>();
                var isBlock = ItemDictionary.Fetch(newRecipe.Id);
                if (isBlock != null)
                    if (isBlock.Name.ToLower().StartsWith("blk_"))
                        newRecipe.Quantity = 128;
                newRecipe.Duration = 0;
                foreach (var requirement in newRecipe.Requirements)
                    newRequirements.Add(new SmRequirement(0, requirement.Id));
                newRecipe.Requirements = newRequirements.ToArray();
                AddRecipeToList(newRecipe);
            }
            MessageBox.Show("Successfully replaced every recipe in this json file.", "Activated noobie mode!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

    }

}