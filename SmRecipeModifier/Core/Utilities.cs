using System;
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

        public static void SetAppTheme(string accent)
        {
            var dictionary = new ResourceDictionary { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{accent}.xaml") };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

    }

}