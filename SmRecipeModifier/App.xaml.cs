using System;
using System.Windows;
using System.Windows.Threading;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;
using SmRecipeModifier.Graphics;

namespace SmRecipeModifier
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }
        internal static Random Randomizer { get; private set; }
        internal static SmItem[] AvailableItems { get; set; }

        private void InitializeApp(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            Randomizer = new Random();
            Utilities.SetAppTheme(Settings.ColorScheme, Settings.EnableDarkMode, false);
            Current.MainWindow = new WnMain();
            Current.MainWindow.Show();
        }

        private void HandleExceptions(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            new WnErrorHandler(args.Exception).ShowDialog();
            Current.Shutdown();
        }

    }

}