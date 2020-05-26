using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using SmRecipeModifier.Core;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModify
    {

        private readonly SmRecipe _original;

        public SmRecipe Result { get; private set; }

        public WnModify(SmRecipe original)
        {
            _original = original;
            InitializeComponent();
            var imagePath = Path.Combine(App.Settings.GameDataPath, Constants.ItemAssetIconFolderPath, $"{_original.Id}.png");
            if (File.Exists(imagePath))
                ImRecipeIcon.Source = new BitmapImage(new Uri(imagePath));
            var item = App.WindowMain.InfoDictionary.Fetch(_original.Id);
            if (item != null) { LbRecipeName.Content = item.Info.Title; }
            else
            {
                item = App.WindowMain.ItemDictionary.Fetch(_original.Id);
                LbRecipeName.Content = item.Name;
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            // todo
            DialogResult = true;
            Close();
        }

    }

}