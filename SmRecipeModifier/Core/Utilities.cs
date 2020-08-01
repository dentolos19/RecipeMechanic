using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Windows;
using ControlzEx.Theming;
using Newtonsoft.Json;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core
{

    public static class Utilities
    {

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
            return recipes?.ToArray();
        }

        public static SmItem[] GetItemsFromJsons(string namesPath, string descriptionsPath)
        {
            var namesData = File.ReadAllText(namesPath);
            var descriptionsData = File.ReadAllText(descriptionsPath);
            var itemNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(namesData);
            var itemDescriptions = JsonConvert.DeserializeObject<Dictionary<string, SmItemDescription>>(descriptionsData);
            if (itemNames == null || itemDescriptions == null)
                return null;
            var items = new List<SmItem>();
            foreach (var (id, name) in itemNames)
            {
                var item = new SmItem();
                foreach (var description in itemDescriptions)
                {
                    if (description.Key != id)
                        continue;
                    if (!string.IsNullOrEmpty(description.Value.Name) && description.Value.Name != name)
                    {
                        item.InGameName = description.Value.Name;
                    }
                    if (!string.IsNullOrEmpty(description.Value.Description))
                    {
                        item.Description = description.Value.Description;
                    }
                }
                item.Name = name;
                item.Id = id;
                items.Add(item);
            }
            return items.ToArray();
        }

        public static SmItem[] MergeRecipesWithItems(SmRecipe[] recipes, SmItem[] items)
        {
            var list = new List<SmItem>();
            foreach (var recipe in recipes)
            {
                foreach (var item in items)
                {
                    if (item.Id.Equals(recipe.Id))
                    {
                        item.Recipe = recipe;
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }

    }

}