using System.Windows;
using RecipeMechanic.Core;
using RecipeMechanic.Views;

namespace RecipeMechanic;

public partial class App
{
    public static Settings Settings { get; } = Settings.Load();

    private void OnStartup(object sender, StartupEventArgs args)
    {
        new MainWindow().Show();
    }

    private void OnExit(object sender, ExitEventArgs args)
    {
        Settings.Save();
    }
}