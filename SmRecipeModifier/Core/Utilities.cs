using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using ControlzEx.Theming;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core
{

    public static class Utilities
    {
        
        private static string? GetSteamLocation()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
            if (key == null)
                return null;
            var installPath = (string)key.GetValue("InstallPath")!;
            return string.IsNullOrEmpty(installPath) ? null : installPath;
        }

        private static string GetSteamAppsLocation(string steamPath)
        {
            var deserialized = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamPath, "steamapps", "libraryfolders.vdf")));
            var values = (VObject)deserialized.Value;
            var value = values["1"]?.Value<string>() ?? string.Empty;
            return value;
        }

        private static bool CheckSteamLocation(string steamPath)
        {
            if (!Directory.Exists(Path.Combine(steamPath, "steamapps", "workshop", "content", Constants.GameId.ToString())))
                return false;
            if (!File.Exists(Path.Combine(steamPath, "steamapps", "common", "Scrap Mechanic", "Release", "ScrapMechanic.exe")))
                return false;
            return true;
        }
        
        public static void RestartApp(string args = null!)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            if (location.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                location = Path.Combine(Path.GetDirectoryName(location)!, Path.GetFileNameWithoutExtension(location) + ".exe");
            Process.Start(location, args);
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

        public static string? DetectGameDataPath()
        {
            var steamPath = GetSteamLocation();
            if (string.IsNullOrEmpty(steamPath))
                return null;
            if (CheckSteamLocation(steamPath))
            {
                return Path.Combine(steamPath, "steamapps", "common", "Scrap Mechanic");
            }
            steamPath = GetSteamAppsLocation(steamPath);
            if (string.IsNullOrEmpty(steamPath))
                return null;
            return CheckSteamLocation(steamPath) ? Path.Combine(steamPath, "steamapps", "common", "Scrap Mechanic") : null;
        }

        public static SmRecipe[] GetRecipesFromJson(string path)
        {
            var data = File.ReadAllText(path);
            var recipes = JsonConvert.DeserializeObject<List<SmRecipe>>(data);
            return recipes.ToArray();
        }
        
        public static SmItem[] GetItemsFromJsons(string gameDescriptionsPath, string survivalDescriptionsPath)
        {
            var gameDescriptionsData = File.ReadAllText(gameDescriptionsPath);
            var survivalDescriptionsData = File.ReadAllText(survivalDescriptionsPath);
            var gameDescriptions = JsonConvert.DeserializeObject<Dictionary<string, SmItemDescription>>(gameDescriptionsData);
            var survivalDescriptions = JsonConvert.DeserializeObject<Dictionary<string, SmItemDescription>>(survivalDescriptionsData);
            var items = new List<SmItem>();
            foreach (var (id, description) in survivalDescriptions)
            {
                items.Add(new SmItem
                {
                    Id = id,
                    Name = description.Name,
                    Description = description.Description
                });
            }
            foreach (var (id, description) in gameDescriptions)
            {
                items.Add(new SmItem
                {
                    Id = id,
                    Name = description.Name,
                    Description = description.Description
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