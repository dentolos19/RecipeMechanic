using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnOpen
    {

        private string _path;

        public string Result { get; private set; }

        public WnOpen()
        {
            InitializeComponent();
            if (!(App.Settings.GameDataPath?.Length > 0))
                return;
            TbGameDataPath.Text = App.Settings.GameDataPath;
            _path = App.Settings.GameDataPath;
            LoadSupportedFiles();
        }

        private void Browse(object sender, RoutedEventArgs args)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == false)
                return;
            TbGameDataPath.Text = dialog.SelectedPath;
            _path = dialog.SelectedPath;
            LoadSupportedFiles();
        }

        private void LoadSupportedFiles()
        {
            LvJsonFiles.Items.Clear();
            var craftBotJsonPath = Path.Combine(_path, Constants.CraftbotJsonPath);
            var dispenserJsonPath = Path.Combine(_path, Constants.DispenserJsonPath);
            var workbenchJsonPath = Path.Combine(_path, Constants.WorkbenchJsonPath);
            if (File.Exists(craftBotJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("craftbot.json", "The well-known craftbot!", craftBotJsonPath));
            if (File.Exists(dispenserJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("dispenser.json", "The one that poops out the craftbot!", dispenserJsonPath));
            if (File.Exists(workbenchJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("workbench.json", "The one that is at the starting ship!", workbenchJsonPath));
        }

        private void Continue(object sender, RoutedEventArgs args)
        {
            if (LvJsonFiles.SelectedItem == null)
            {
                MessageBox.Show("Select a json file in the list to continue!", "You forgot something!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            var item = LvJsonFiles.SelectedItem as LvJsonBinding;
            Result = item?.Path;
            DialogResult = true;
            App.Settings.GameDataPath = TbGameDataPath.Text;
            App.Settings.Save();
            Close();
        }

    }

}