using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
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
                RecipeListItemAmountText.Text = $"There are a total of {items.Length} survival items in the file!";
            }
        }

        private void Save(object sender, ExecutedRoutedEventArgs args)
        {
            // TODO
        }

        private void SaveAs(object sender, ExecutedRoutedEventArgs args)
        {
            // TODO
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void CreateBackup(object sender, RoutedEventArgs args)
        {
            if (File.Exists(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupFilePath)) && MessageBox.Show("A backup already exist, do you want to overwrite it?", "SmRecipeModifier", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            try
            {
                if (File.Exists(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupFilePath)))
                    File.Delete(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupFilePath));
                ZipFile.CreateFromDirectory(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupTargetFolderPath), Path.Combine(App.Settings.GameDataPath));
            }
            catch
            {
                MessageBox.Show("Backup creation failed!", "SmRecipeModifier");
                return;
            }
            MessageBox.Show("Backup successfully created!", "SmRecipeModifier");
        }

        private void ApplyBackup(object sender, RoutedEventArgs args)
        {
            if (!File.Exists(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupFilePath)))
            {
                MessageBox.Show("Backup file not found!", "SmRecipeModifier");
                return;
            }
            try
            {
                ZipFile.ExtractToDirectory(Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupFilePath), Path.Combine(App.Settings.GameDataPath, Constants.ScrapMechanicBackupTargetFolderPath), true);
            }
            catch
            {
                MessageBox.Show("Unable to apply backup!", "SmRecipeModifier");
                return;
            }
            MessageBox.Show("Applied backup successfully!");
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
            // TODO
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void ModifyRecipe(object sender, RoutedEventArgs args)
        {
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

    }

}