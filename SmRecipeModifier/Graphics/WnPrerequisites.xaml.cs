using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using SmRecipeModifier.Core;

namespace SmRecipeModifier.Graphics
{

    public partial class WnPrerequisites
    {

        public WnPrerequisites()
        {
            InitializeComponent();
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

        private void Continue(object sender, RoutedEventArgs args)
        {
            if (string.IsNullOrEmpty(GameDataPathBox.Text))
            {
                MessageBox.Show("The game data path input box must not be empty!", Application.Current.Resources["String_DialogWinTitle"].ToString());
                return;
            }
            App.Settings.GameDataPath = GameDataPathBox.Text;
            App.Settings.Save();
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs args)
        {
            Close();
        }

    }

}