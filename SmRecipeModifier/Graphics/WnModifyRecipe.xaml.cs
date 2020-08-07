using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Bindings;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRecipe
    {

        public SmRecipe RecipeResult { get; }

        public WnModifyRecipe(SmRecipe recipe)
        {
            RecipeResult = recipe;
            InitializeComponent();
            QuantityBox.Value = recipe.Quantity;
            DurationBox.Value = recipe.Duration;
            foreach (var requirement in recipe.Requirements)
                RequirementList.Items.Add(new RequirementItemBinding(requirement));
        }

        private void Continue(object sender, RoutedEventArgs args)
        {
            RecipeResult.Quantity = (int)QuantityBox.Value!;
            RecipeResult.Duration = (int)DurationBox.Value!;
            RecipeResult.Requirements = RequirementList.Items.OfType<RequirementItemBinding>().Select(requirement => requirement.Requirement).ToArray();
            DialogResult = true;
            Close();
        }

        private void CopyId(object sender, RoutedEventArgs args)
        {
            var item = (RequirementItemBinding)RequirementList.SelectedItem;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
        }

        private void AddRequirement(object sender, RoutedEventArgs args)
        {
            var dialog = new WnModifyRequirement { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            RequirementList.Items.Add(new RequirementItemBinding(dialog.RequirementResult));
            RequirementList.SelectedIndex = RequirementList.Items.Count - 1;
        }

        private void RemoveRequirement(object sender, RoutedEventArgs args)
        {
            if (RemoveButton.IsEnabled == false)
                return;
            var item = (RequirementItemBinding)RequirementList.SelectedItem;
            if (item == null)
                return;
            if (MessageBox.Show("Are you sure that you want to remove this requirement?", "SmRecipeModifier", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                RequirementList.Items.Remove(item);
        }

        private void ModifyRequirement(object sender, RoutedEventArgs args)
        {
            var item = (RequirementItemBinding)RequirementList.SelectedItem;
            if (item == null)
                return;
            var dialog = new WnModifyRequirement(item.Requirement) { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            RequirementList.Items.Remove(item);
            RequirementList.Items.Add(new RequirementItemBinding(dialog.RequirementResult));
            RequirementList.SelectedIndex = RequirementList.Items.Count - 1;
        }

        private void UpdateSelection(object sender, SelectionChangedEventArgs args)
        {
            if (RequirementList.SelectedItem == null)
            {
                RemoveButton.IsEnabled = false;
                ModifyButton.IsEnabled = false;
            }
            else
            {
                RemoveButton.IsEnabled = true;
                ModifyButton.IsEnabled = true;
            }
        }

    }

}