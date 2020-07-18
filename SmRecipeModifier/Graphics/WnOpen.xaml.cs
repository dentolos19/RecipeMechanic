<<<<<<< Updated upstream
﻿using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;
=======
﻿using System.Windows;
using Ookii.Dialogs.Wpf;
>>>>>>> Stashed changes

namespace SmRecipeModifier.Graphics
{

    public partial class WnOpen
    {

        private string _path;

        public WnOpen()
        {
            InitializeComponent();
            if (!(App.Settings.GameDataPath?.Length > 0))
                return;
            TbGameDataPath.Text = App.Settings.GameDataPath;
            _path = App.Settings.GameDataPath;
            LoadSupportedFiles();
        }

        public string Result { get; private set; }

        private void Browse(object sender, RoutedEventArgs args)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == false)
                return;
            TbGameDataPath.Text = dialog.SelectedPath;
            _path = dialog.SelectedPath;
            LoadSupportedFiles();
        }
        
        private void LoadFiles(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void LoadSupportedFiles()
        {
<<<<<<< Updated upstream
            LvJsonFiles.Items.Clear();
            var cookBotJsonPath = Path.Combine(_path, Constants.CookbotJsonPath);
            var craftBotJsonPath = Path.Combine(_path, Constants.CraftbotJsonPath);
            var dispenserJsonPath = Path.Combine(_path, Constants.DispenserJsonPath);
            var hideoutJsonPath = Path.Combine(_path, Constants.HideoutJsonPath);
            var undecidedJsonPath = Path.Combine(_path, Constants.UndecidedJsonPath);
            var workbenchJsonPath = Path.Combine(_path, Constants.WorkbenchJsonPath);
            if (File.Exists(cookBotJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("cookbot.json", "Your personal non-living chef!", cookBotJsonPath));
            if (File.Exists(craftBotJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("craftbot.json", "The well-known CraftBot!", craftBotJsonPath));
            if (File.Exists(dispenserJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("dispenser.json", "The one that creates out the CraftBot!", dispenserJsonPath));
            if (File.Exists(hideoutJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("hideout.json", "The person who sells things overpriced!", hideoutJsonPath));
            if (File.Exists(undecidedJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("undecided.json", "Try to discover what this is!", undecidedJsonPath));
            if (File.Exists(workbenchJsonPath))
                LvJsonFiles.Items.Add(new LvJsonBinding("workbench.json", "The one that is at the starting ship!", workbenchJsonPath));
=======
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this) == true)
            {
                GameDataPathBox.Text = dialog.SelectedPath;
            }
>>>>>>> Stashed changes
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