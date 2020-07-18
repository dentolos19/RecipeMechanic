using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnNewRecipe
    {

        private SmRecipe _recipeData;
        private SmRecipe _recipeRequirements;

        public WnNewRecipe()
        {
            InitializeComponent();
            foreach (var item in App.WindowMain.ItemDictionary.Items)
                CbItem.Items.Add(new ComboBoxItem { Content = item.Name, Tag = item.Id });
            CbItem.SelectedIndex = 0;
        }

        public SmRecipe Result { get; private set; }

        private void Create(object sender, RoutedEventArgs args)
        {
            if (_recipeData == null || _recipeRequirements == null)
            {
                MessageBox.Show("The recipe values for data or requirement are null, please modify them.", "Don't save without me!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            Result = new SmRecipe
            {
                Id = ((ComboBoxItem) CbItem.SelectedItem).Tag.ToString(),
                Duration = _recipeData.Duration,
                Quantity = _recipeData.Quantity,
                Requirements = _recipeRequirements.Requirements
            };
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void ModifyRecipeData(object sender, RoutedEventArgs args)
        {
            var dialog = new WnModify(null, true) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _recipeData = new SmRecipe();
            _recipeData.Quantity = dialog.Result.Quantity;
            _recipeData.Duration = dialog.Result.Duration;
            BnModifyRecipeData.Content = "Modify Recipe Data (Set)";
        }

        private void ModifyRecipeRequirements(object sender, RoutedEventArgs args)
        {
            var dialog = new WnModify(null, false, true) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _recipeRequirements = new SmRecipe();
            _recipeRequirements.Requirements = dialog.Result.Requirements;
            BnModifyRecipeRequirements.Content = "Modify Recipe Requirements (Set)";
        }

    }

}