﻿using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnNewRecipe
    {

        public SmItem ItemResult { get; private set; }

        public WnNewRecipe()
        {
            InitializeComponent();
            foreach (var item in App.AvailableItems)
                ItemListComboBox.Items.Add(new ComboBoxItem { Content = item.InGameName ?? item.Name, Tag = item });
            ItemListComboBox.SelectedIndex = 0;
        }

        private void Create(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
            Close();
        }

        private void Modify(object sender, RoutedEventArgs args)
        {
            var dialog = new WnModifyRecipe(ItemResult.Recipe){Owner = this};
            if (dialog.ShowDialog() == true)
                ItemResult.Recipe = dialog.RecipeResult;
        }

        private void UpdateSelection(object sender, SelectionChangedEventArgs args)
        {
            var item = (ComboBoxItem)ItemListComboBox.SelectedItem;
            if (item == null)
                return;
            ItemResult = (SmItem)item.Tag;
        }

    }

}