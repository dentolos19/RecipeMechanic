using System.Windows;
using SmRecipeModifier.Core;
using SmRecipeModifier.Graphics;

namespace SmRecipeModifier
{
    
    public partial class App
    {

        public static Configuration Settings { get; } = Configuration.Load();

        private void Initialize(object sender, StartupEventArgs args)
        {
            Utilities.SetAppTheme(Settings.AccentName, Settings.EnableDarkMode);
            new WnMain().Show();
        }

    }

}