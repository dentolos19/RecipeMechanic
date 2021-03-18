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
            EnableNameFilterSwitch.IsChecked = App.Settings.EnableNameFilter;
            EnableIdFilterSwitch.IsChecked = App.Settings.EnableIdFilter;
            EnableDescriptionFilterSwitch.IsChecked = App.Settings.EnableDescriptionFilter;
        }

        private void SaveSettings(object sender, RoutedEventArgs args)
        {
            App.Settings.GameDataPath = GameDataPathBox.Text;
            App.Settings.ColorScheme = ColorSchemeBox.Text;
            App.Settings.EnableDarkMode = EnableDarkModeSwitch.IsChecked == true;
            App.Settings.EnableNameFilter = EnableNameFilterSwitch.IsChecked == true;
            App.Settings.EnableIdFilter = EnableIdFilterSwitch.IsChecked == true;
            App.Settings.EnableDescriptionFilter = EnableDescriptionFilterSwitch.IsChecked == true;
            App.Settings.Save();
            Utilities.SetAppTheme(App.Settings.ColorScheme, App.Settings.EnableDarkMode);
            MessageBox.Show("All settings has been saved!", Application.Current.Resources["String_DialogWinTitle"].ToString());
            Close();
        }
        
        private void ResetSettings(object sender, RoutedEventArgs args)
        {
            if (MessageBox.Show("Are you sure that you want to reset settings?", Application.Current.Resources["String_DialogWinTitle"].ToString(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            App.Settings.Reset();
            Utilities.RestartApp();
        }

        private void BrowseGameDataPath(object sender, RoutedEventArgs args)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() != true)
                return;
            if (!File.Exists(Path.Combine(dialog.SelectedPath, Constants.ScrapMechanicExePath)))
            {
                MessageBox.Show("This path doesn't contain the game executable!", Application.Current.Resources["String_DialogWinTitle"].ToString());
                return;
            }
            GameDataPathBox.Text = dialog.SelectedPath;
        }

    }

}