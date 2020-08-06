using System.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SmRecipeModifier.Core;
using SmRecipeModifier.Graphics;

namespace SmRecipeModifier
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        internal static WnMain WindowMain { get; private set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            AppCenter.Start("deff7951-472f-4983-9d9e-cb073440e574", typeof(Analytics), typeof(Crashes));
            Utilities.SetAppTheme(Settings.ColorScheme, Settings.EnableDarkMode, false);
            WindowMain = new WnMain();
            WindowMain.Show();
        }

    }

}