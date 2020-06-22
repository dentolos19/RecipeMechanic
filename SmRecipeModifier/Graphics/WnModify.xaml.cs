using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModify
    {

        private readonly bool _massEditing;
        private readonly SmRecipe _original;

        public WnModify(SmRecipe original, bool massEditing = false)
        {
            _original = original;
            InitializeComponent();
            if (massEditing)
            {
                _massEditing = true;
                LbRecipeName.Content = "Mass Editing Mode";
                TiRequirements.IsEnabled = false;
                return;
            }
            var item = App.WindowMain.InfoDictionary.Fetch(_original.Id);
            if (item != null)
            {
                LbRecipeName.Content = item.Info.Title;
            }
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
            if (!_massEditing)
            {
                var result = _original;
                result.Quantity = int.Parse(TbQuantity.Text);
                result.Duration = int.Parse(TbDuration.Text);
                result.Requirements = LvRequirements.Items.OfType<LvRequirementBinding>().Select(requirement => new SmRequirement(requirement.Quantity, requirement.Id)).ToArray();
                Result = result;
            }
            else
            {
                var result = new SmRecipe { Quantity = int.Parse(TbQuantity.Text), Duration = int.Parse(TbDuration.Text) };
                Result = result;
            }
            DialogResult = true;
            Close();
        }

        private void Add(object sender, RoutedEventArgs args)
        {
            if (LvRequirements.Items.Count >= 3)
            {
                MessageBox.Show("You are only allowed max 3 requirements in a recipe.", "Scrap Mechanic doesn't allow that!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            MessageBox.Show("This function is disabled.", "Sorry about that!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Remove(object sender, RoutedEventArgs args)
        {
            if (LvRequirements.SelectedItem == null)
                return;
            if (LvRequirements.Items.Count == 1)
            {
                MessageBox.Show("You must have a minimum 1 requirement in a recipe.", "Scrap Mechanic doesn't allow that!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBox.Show("This function is not available yet.", "Sorry about that!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void RemoveRq(LvRequirementBinding binding)
        {
            lock (LvRequirements.Items)
            {
                foreach (var item in LvRequirements.Items.OfType<LvRequirementBinding>().ToArray())
                    if (item.Id == binding.Id)
                        LvRequirements.Items.Remove(item);
            }
        }

        private bool CheckRqIdExists(string id)
        {
            var result = false;
            foreach (var item in LvRequirements.Items.OfType<LvRequirementBinding>())
                if (item.Id == id)
                    result = true;
            return result;
        }

        private void Modify(object sender, RoutedEventArgs args)
        {
            if (LvRequirements.SelectedItem == null)
                return;
            var binding = LvRequirements.SelectedItem as LvRequirementBinding;
            var dialog = new WnModifyRq(binding) { Owner = this };
            if (dialog.ShowDialog() == false || dialog.Result == null)
                return;
            RemoveRq(binding);
            if (CheckRqIdExists(binding?.Id))
            {
                MessageBox.Show("Duplicated requirement found! Please use another item.", "There can't be both!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                LvRequirements.Items.Add(binding);
                return;
            }
            LvRequirements.Items.Add(dialog.Result);
        }

        private void Cancel(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void ItemSelectionUpdate(object sender, SelectionChangedEventArgs args)
        {
            if (LvRequirements.SelectedItem == null)
            {
                BnRemove.IsEnabled = false;
                BnModify.IsEnabled = false;
            }
            else
            {
                BnRemove.IsEnabled = true;
                BnModify.IsEnabled = true;
            }
        }

    }

}