using System.Windows;
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
                ItemList.Items.Add(new ComboBoxItem { Content = item.Name, Tag = item });
            ItemList.SelectedIndex = App.Randomizer.Next(ItemList.Items.Count - 1);;
        }

        private void Create(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
            Close();
        }

        private void Modify(object sender, RoutedEventArgs args)
        {
            var dialog = new WnModifyRecipe(ItemResult.Recipe) { Owner = this };
            if (dialog.ShowDialog() == true)
                ItemResult.Recipe = dialog.RecipeResult;
        }

        private void UpdateSelection(object sender, SelectionChangedEventArgs args)
        {
            if (ItemList.SelectedItem is ComboBoxItem item)
                ItemResult = (SmItem)item.Tag;
        }

    }

}