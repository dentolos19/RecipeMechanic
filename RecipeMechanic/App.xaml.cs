using RecipeMechanic.Core;
using System.Windows;

namespace RecipeMechanic;

public partial class App
{

    public static Settings Settings { get; } = Settings.Load();

    private void OnExit(object sender, ExitEventArgs args)
    {
        Settings.Save();
    }

}