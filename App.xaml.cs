using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using SmRecipeModifier.Core;
using SmRecipeModifier.Graphics;

namespace SmRecipeModifier
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            Settings = Configuration.Load();
            new WnMain().Show();
        }

        private void HandleExceptions(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var answer = MessageBox.Show($"An error had occurred! {args.Exception.Message} Do you want to restart? Or else this program will just exit.", "Houston, we got a problem!", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (answer != MessageBoxResult.Yes)
                return;
            args.Handled = true;
            Process.Start(Assembly.GetExecutingAssembly().Location);
            Current.Shutdown();
        }

    }

}