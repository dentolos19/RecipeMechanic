using System.Collections.Generic;
using System.IO;
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
        public SmItemDictionary ItemDictionary;
        public SmItemInfoDictionary InfoDictionary;

        public WnMain()
        {
            InitializeComponent();
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
            if (info != null) { LvRecipes.Items.Add(new LvRecipeBinding(info.Info.Title, info.Id, recipe)); }
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
            if (MessageBox.Show("Are you sure? If you want to revert back to the original game then you should verify the files via steam.", "Think twice about it!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

    }

}