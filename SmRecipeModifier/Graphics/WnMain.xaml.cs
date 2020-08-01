using System.IO;
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

        public WnMain()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(App.Settings.GameDataPath))
                new WnIntro().ShowDialog();
        }

        private void Open(object sender, ExecutedRoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _selectedPath = dialog.SelectedPath;
                FileNameBox.Text = Path.GetFileName(_selectedPath)!;
                // TODO
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
            // TODO
        }

        private void ApplyBackup(object sender, RoutedEventArgs args)
        {
            // TODO
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