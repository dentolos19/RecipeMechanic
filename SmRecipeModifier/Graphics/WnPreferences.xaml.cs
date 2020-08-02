using System.IO;
using System.Windows;
using ControlzEx.Theming;
using Ookii.Dialogs.Wpf;
using SmRecipeModifier.Core;

namespace SmRecipeModifier.Graphics
{

    public partial class WnPreferences
    {

        public WnPreferences()
        {
            InitializeComponent();
            foreach (var color in ThemeManager.Current.ColorSchemes)
                ColorSchemeBox.Items.Add(color);
            GameDataPathBox.Text = App.Settings.GameDataPath;
            ColorSchemeBox.Text = App.Settings.ColorScheme;
            EnableDarkModeSwitch.IsChecked = App.Settings.EnableDarkMode;
        }

        private void SaveSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.GameDataPath = GameDataPathBox.Text;
            App.Settings.ColorScheme = ColorSchemeBox.Text;
            App.Settings.EnableDarkMode = EnableDarkModeSwitch.IsChecked == true;
            App.Settings.Save();
            Utilities.SetAppTheme(App.Settings.ColorScheme, App.Settings.EnableDarkMode);
        }

        private void BrowseGameDataPath(object sender, RoutedEventArgs args)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                if (!File.Exists(Path.Combine(dialog.SelectedPath, Constants.ScrapMechanicExePath)))
                {
                    MessageBox.Show("This path doesn't contain the game executable!", "SmRecipeModifier");
                    return;
                }
                GameDataPathBox.Text = dialog.SelectedPath;
            }
        }

    }

}