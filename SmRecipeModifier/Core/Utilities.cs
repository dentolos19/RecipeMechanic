using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using ControlzEx.Theming;
using Newtonsoft.Json;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core
{

    public static class Utilities
    {
        
        public static void RestartApp(string args = null)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            if (location.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                location = Path.Combine(Path.GetDirectoryName(location)!, Path.GetFileNameWithoutExtension(location) + ".exe");
            Process.Start(location, args ?? string.Empty);
            Application.Current.Shutdown();
        }

        public static void SetAppTheme(string colorScheme, bool darkMode, bool afterInit = true)
        {
            if (afterInit)
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, darkMode ? ThemeManager.BaseColorDarkConst : ThemeManager.BaseColorLightConst);
            }
            else
            {
                var dictionary = new ResourceDictionary
                {
                    Source = darkMode
                        ? new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{colorScheme}.xaml")
                        : new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{colorScheme}.xaml")
                };
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
        }

        public static SmRecipe[] GetRecipesFromJson(string path)
        {
            var data = File.ReadAllText(path);
            var recipes = JsonConvert.DeserializeObject<List<SmRecipe>>(data);
            return recipes.ToArray();
        }
        
        public static SmItem[] GetItemsFromJsons(string itemDescriptionsPath, string descriptionsPath)
        {
            var itemDescriptionsData = File.ReadAllText(itemDescriptionsPath);
            var descriptionsData = File.ReadAllText(descriptionsPath);
            var itemDescriptions = JsonConvert.DeserializeObject<Dictionary<string, SmItemDescription>>(itemDescriptionsData);
            var descriptions = JsonConvert.DeserializeObject<Dictionary<string, SmItemDescription>>(descriptionsData);
            var items = new List<SmItem>();
            foreach (var description in descriptions)
            {
                items.Add(new SmItem
                {
                    Id = description.Key,
                    Name = description.Value.Name,
                    Description = description.Value.Description
                });
            }
            foreach (var description in itemDescriptions)
            {
                items.Add(new SmItem
                {
                    Id = description.Key,
                    Name = description.Value.Name,
                    Description = description.Value.Description
                });
            }
            return items.Distinct(new SmItem.Comparer()).ToArray();
        }

        public static SmItem[] MergeRecipesWithItems(SmRecipe[] recipes, SmItem[] items)
        {
            var list = new List<SmItem>();
            foreach (var recipe in recipes)
            {
                foreach (var item in items)
                {
                    if (!item.Id.Equals(recipe.Id))
                        continue;
                    item.Recipe = recipe;
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

    }

}