using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnMain
    {

        private string _path;
        private SmItemDictionary _itemDictionary;
        private SmItemInfoDictionary _infoDictionary;

        public WnMain()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _path = dialog.FileName;
            TbFileName.Text = Path.GetFileName(_path)!;
            BnSave.IsEnabled = true;
            BnSaveAs.IsEnabled = true;
            MiSave.IsEnabled = true;
            MiSaveAs.IsEnabled = true;
            BnAddRecipe.IsEnabled = true;
            LvRecipes.Items.Clear();
            _infoDictionary = new SmItemInfoDictionary(Path.Combine(App.Settings.GameDataPath, Constants.ItemDescriptionsJsonPath));
            _itemDictionary = new SmItemDictionary(Path.Combine(App.Settings.GameDataPath, Constants.ItemNamesJsonPath));
            var recipes = new SmRecipeDictionary(_path).Items;
            foreach (var recipe in recipes)
            {
                var info = _infoDictionary.Fetch(recipe.Id);
                if (info != null) { LvRecipes.Items.Add(new LvRecipeBinding(info.Info.Title, info.Id, recipe)); }
                else
                {
                    info = _itemDictionary.Fetch(recipe.Id);
                    if (info != null)
                        LvRecipes.Items.Add(new LvRecipeBinding(info.Name, info.Id, recipe));
                }
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
            MessageBox.Show("This function is not yet implemented.", "Sorry about that!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void ModifyRecipe(object sender, RoutedEventArgs args)
        {
            // todo
        }

    }

}