using System.Windows;
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
                RequirementList.Items.Add(RequirementItemBinding.ConvertFromRequirement(requirement));
        }

        private void Continue(object sender, RoutedEventArgs args)
        {
            RecipeResult.Quantity = (int)QuantityBox.Value!;
            RecipeResult.Duration = (int)DurationBox.Value!;
            // TODO
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
            // TODO
        }

        private void RemoveRequirement(object sender, RoutedEventArgs args)
        {
            // TODO
        }

        private void ModifyRequirement(object sender, RoutedEventArgs args)
        {
            // TODO
        }

    }

}