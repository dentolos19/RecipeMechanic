using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SmRecipeModifier.Core
{

    public static class Utilities
    {

        public static string GetRandomAccent()
        {
            var accents = new[] { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
            var random = new Random();
            return accents[random.Next(accents.Length)];
        }
<<<<<<< Updated upstream

        public static void SetAppTheme(string accent)
        {
            var dictionary = new ResourceDictionary { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{accent}.xaml") };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

=======
        
        public static void RestartApp(string args = null)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            if (location.ToLower().EndsWith(".dll"))
                location = Path.Combine(Path.GetDirectoryName(location)!, Path.GetFileNameWithoutExtension(location) + ".exe");
            Process.Start(location, args);
            Application.Current.Shutdown();
        }
        
>>>>>>> Stashed changes
    }

}