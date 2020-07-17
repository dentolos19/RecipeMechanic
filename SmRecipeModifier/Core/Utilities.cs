using System;
using System.Windows;

namespace SmRecipeModifier.Core
{

    public static class Utilities
    {

        public static void SetAppTheme(string accent, bool darkMode)
        {
            var dictionary = new ResourceDictionary
            {
                Source = darkMode
                    ? new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{accent}.xaml")
                    : new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{accent}.xaml")
            };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }


    }

}