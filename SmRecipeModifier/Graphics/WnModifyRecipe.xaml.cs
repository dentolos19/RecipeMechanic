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
            if (recipe == null)
                RecipeResult = new SmRecipe { Quantity = 0, Duration = 0 };
            QuantityBox.Value = RecipeResult.Quantity;
            DurationBox.Value = RecipeResult.Duration;
            if (RecipeResult.Requirements == null || RecipeResult.Requirements.Length < 1)
                RecipeResult.Requirements = new[] { new SmRequirement { Id = App.AvailableItems[App.Randomizer.Next(App.AvailableItems.Length)].Id, Quantity = 0 } };
            foreach (var requirement in RecipeResult.Requirements)
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
            if (!RemoveButton.IsEnabled)
                return;
            var item = (RequirementItemBinding)RequirementList.SelectedItem;
            if (item == null)
                return;
            if (RequirementList.Items.Count == 1)
            {
                MessageBox.Show("A recipe must have at least one requirement!", Application.Current.Resources["String_DialogWinTitle"].ToString());
                return;
            }
            if (MessageBox.Show("Are you sure that you want to remove this requirement?", Application.Current.Resources["String_DialogWinTitle"].ToString(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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