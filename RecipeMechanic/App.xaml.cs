using System.Windows;
using System.Windows.Threading;
using RecipeMechanic.Core;

namespace RecipeMechanic;

public partial class App
{

    public static Settings Settings { get; } = Settings.Load();

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
        MessageBox.Show("An unhandled exception occurred! " + args.Exception.Message, "Recipe Mechanic");
        args.Handled = true;
    }

    private void OnExit(object sender, ExitEventArgs args)
    {
        Settings.Save();
    }

}