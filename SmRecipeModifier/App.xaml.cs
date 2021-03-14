using System;
using System.Windows;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;
using SmRecipeModifier.Graphics;

namespace SmRecipeModifier
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }
        internal static Random Randomizer { get; private set; }

        internal static WnMain WindowMain { get; private set; }

        internal static SmItem[] AvailableItems { get; set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            Randomizer = new Random();
            Utilities.SetAppTheme(Settings.ColorScheme, Settings.EnableDarkMode, false);
            WindowMain = new WnMain();
            WindowMain.Show();
        }

    }

}