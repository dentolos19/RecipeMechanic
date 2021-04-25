using System.Diagnostics;
using System.IO;
using System.Windows;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Bindings;

namespace SmRecipeModifier.Graphics
{

    public partial class WnOpenRecipe
    {

        public WnOpenRecipe()
        {
            InitializeComponent();
            JsonFileList.Items.Clear();
            var cookBotJsonPath = Path.Combine(App.Settings.GameDataPath, Constants.CookbotJsonPath);
            var craftBotJsonPath = Path.Combine(App.Settings.GameDataPath, Constants.CraftbotJsonPath);
            var dispenserJsonPath = Path.Combine(App.Settings.GameDataPath, Constants.DispenserJsonPath);
            var hideoutJsonPath = Path.Combine(App.Settings.GameDataPath, Constants.HideoutJsonPath);
            var workbenchJsonPath = Path.Combine(App.Settings.GameDataPath, Constants.WorkbenchJsonPath);
            if (File.Exists(cookBotJsonPath))
                JsonFileList.Items.Add(new JsonItemBinding("cookbot.json", "Your personal non-living chef!", cookBotJsonPath));
            if (File.Exists(craftBotJsonPath))
                JsonFileList.Items.Add(new JsonItemBinding("craftbot.json", "The well-known CraftBot!", craftBotJsonPath));
            if (File.Exists(dispenserJsonPath))
                JsonFileList.Items.Add(new JsonItemBinding("dispenser.json", "The one that creates the CraftBot!", dispenserJsonPath));
            if (File.Exists(hideoutJsonPath))
                JsonFileList.Items.Add(new JsonItemBinding("hideout.json", "The person who sells things overpriced!", hideoutJsonPath));
            if (File.Exists(workbenchJsonPath))
                JsonFileList.Items.Add(new JsonItemBinding("workbench.json", "The one that is at the starting ship!", workbenchJsonPath));
        }

        public string SelectedPath { get; private set; }

        private void Continue(object sender, RoutedEventArgs args)
        {
            var item = (JsonItemBinding)JsonFileList.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Select a JSON file before continuing!", Application.Current.Resources["String_DialogWinTitle"].ToString());
                return;
            }
            SelectedPath = item.Path;
            DialogResult = true;
            Close();
        }

        private void OpenFileLocation(object sender, RoutedEventArgs args)
        {
            if (JsonFileList.SelectedItem is JsonItemBinding binding)
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{binding.Path}\""
                });
        }

    }

}