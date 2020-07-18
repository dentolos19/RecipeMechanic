using System.Windows;
using Ookii.Dialogs.Wpf;
using SmRecipeModifier.Core;

namespace SmRecipeModifier.Graphics
{

    public partial class WnPreferences
    {

        public WnPreferences()
        {
            InitializeComponent();
        }
        
        private void LoadSettings(object sender, RoutedEventArgs args)
        {
            GameDataPathBox.Text = App.Settings.GameDataPath;
            AccentNameBox.Text = App.Settings.AccentName;
            EnableDarkModeSwitch.IsChecked = App.Settings.EnableDarkMode;
            EnableDeveloperAnalyticsSwitch.IsChecked = App.Settings.EnableDeveloperAnalytics;
        }

        private void SaveSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.GameDataPath = GameDataPathBox.Text;
            App.Settings.AccentName = AccentNameBox.Text;
            App.Settings.EnableDarkMode = EnableDarkModeSwitch.IsChecked == true;
            App.Settings.EnableDeveloperAnalytics = EnableDeveloperAnalyticsSwitch.IsChecked == true;
            App.Settings.Save();
            if (MessageBox.Show("All settings has been saved, do you want to restart this program?", "SmRecipeModifier") == MessageBoxResult.Yes)
                Utilities.RestartApp();
        }
        
        private void ResetSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.Reset();
            MessageBox.Show("All settings has been reset to their default settings, this program will now restart!", "SmRecipeModifier");
            Utilities.RestartApp();
        }
        
        private void BrowseGameDataPath(object sender, RoutedEventArgs args)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this) == true)
                GameDataPathBox.Text = dialog.SelectedPath;
        }

    }

}