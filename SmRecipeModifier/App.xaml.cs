using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
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
            var accent = Utilities.GetRandomAccent();
            Utilities.SetAppTheme(accent);
            AppCenter.Start("deff7951-472f-4983-9d9e-cb073440e574", typeof(Analytics), typeof(Crashes));
            WindowMain = new WnMain();
            WindowMain.Show();
        }

    }

}