using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModify
    {

        private readonly SmRecipe _original;

        public WnModify(SmRecipe original)
        {
            _original = original;
            InitializeComponent();
            var item = App.WindowMain.InfoDictionary.Fetch(_original.Id);
            if (item != null) { LbRecipeName.Content = item.Info.Title; }
            else
            {
                item = App.WindowMain.ItemDictionary.Fetch(_original.Id);
                LbRecipeName.Content = item.Name;
            }
            TbDuration.Text = _original.Duration.ToString();
            TbQuantity.Text = _original.Quantity.ToString();
            foreach (var requirement in _original.Requirements)
                LvRequirements.Items.Add(new LvRequirementBinding(requirement.Quantity, App.WindowMain.ItemDictionary.Fetch(requirement.Id).Name, requirement.Id));
        }

        public SmRecipe Result { get; private set; }

        private void Save(object sender, RoutedEventArgs e)
        {
            var result = _original;
            result.Quantity = int.Parse(TbQuantity.Text);
            result.Duration = int.Parse(TbDuration.Text);
            var newRequirements = new List<SmRequirement>();
            foreach (var requirement in LvRequirements.Items.OfType<LvRequirementBinding>())
                newRequirements.Add(new SmRequirement(requirement.Quantity, requirement.Id));
            result.Requirements = newRequirements.ToArray();
            Result = result;
            DialogResult = true;
            Close();
        }

        private void Add(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void Remove(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void Modify(object sender, RoutedEventArgs args)
        {
            // todo
        }

    }

}