using System;
using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRequirement
    {

        public SmRequirement RequirementResult { get; }

        public WnModifyRequirement(SmRequirement requirement = null)
        {
            RequirementResult = requirement;
            InitializeComponent();
            var index = -1;
            var count = -1;
            foreach (var item in App.AvailableItems)
            {
                count++;
                if (requirement != null && item.Id.Equals(requirement.Id))
                    index = count;
                ItemList.Items.Add(new ComboBoxItem { Content = item.InGameName ?? item.Name, Tag = item });
            }
            ItemList.SelectedIndex = App.Randomizer.Next(ItemList.Items.Count - 1);
            if (requirement == null)
            {
                RequirementResult = new SmRequirement();
                QuantityBox.Value = 0;
            }
            QuantityBox.Value = RequirementResult.Quantity;
            if (!(index <= 0))
                ItemList.SelectedIndex = index;
        }

        private void Continue(object sender, RoutedEventArgs args)
        {
            if (ItemList.SelectedItem is ComboBoxItem item)
            {
                RequirementResult.Quantity = (int)QuantityBox.Value!;
                RequirementResult.Id = ((SmItem)item.Tag).Id;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Select a proper in-game item!", "SmRecipeModifier");
            }
            
        }

    }

}