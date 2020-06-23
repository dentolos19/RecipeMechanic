using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModify
    {

        private readonly bool _massDataEditing;
        private readonly bool _massRequirementEditing;
        private readonly SmRecipe _original;

        public WnModify(SmRecipe original, bool massDataEditing = false, bool massRequirementEditing = false)
        {
            _original = original;
            InitializeComponent();
            if (massDataEditing)
            {
                _massDataEditing = true;
                LbRecipeName.Content = "Mass Data Editing Mode";
                TiData.IsEnabled = true;
                TiRequirements.IsEnabled = false;
                return;
            }
            if (massRequirementEditing)
            {
                _massRequirementEditing = true;
                LbRecipeName.Content = "Mass Requirement Editing Mode";
                TiData.IsEnabled = false;
                TiRequirements.IsEnabled = true;
                TiRequirements.IsSelected = true;
                LvRequirements.Items.Add(new LvRequirementBinding(1, App.WindowMain.ItemDictionary.Fetch("1fc74a28-addb-451a-878d-c3c605d63811").Name, "1fc74a28-addb-451a-878d-c3c605d63811"));
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
            if (_massDataEditing)
            {
                var result = new SmRecipe { Quantity = int.Parse(TbQuantity.Text), Duration = int.Parse(TbDuration.Text) };
                Result = result;
            }
            else if (_massRequirementEditing)
            {
                var result = new SmRecipe { Requirements = LvRequirements.Items.OfType<LvRequirementBinding>().Select(requirement => new SmRequirement(requirement.Quantity, requirement.Id)).ToArray() };
                Result = result;
            }
            else
            {
                var result = _original;
                result.Quantity = int.Parse(TbQuantity.Text);
                result.Duration = int.Parse(TbDuration.Text);
                result.Requirements = LvRequirements.Items.OfType<LvRequirementBinding>().Select(requirement => new SmRequirement(requirement.Quantity, requirement.Id)).ToArray();
                Result = result;
            }
            DialogResult = true;
            Close();
        }

        private void Add(object sender, RoutedEventArgs args)
        {
            if (LvRequirements.Items.Count >= 4)
            {
                MessageBox.Show("You are only allowed max 4 requirements in a recipe.", "Scrap Mechanic doesn't allow that!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            var binding = new LvRequirementBinding(1, App.WindowMain.ItemDictionary.Fetch("1fc74a28-addb-451a-878d-c3c605d63811").Name, "1fc74a28-addb-451a-878d-c3c605d63811");
            var dialog = new WnModifyRq(binding, true) { Owner = this };
            if (dialog.ShowDialog() == false || dialog.Result == null)
                return;
            if (CheckRqIdExists(binding.Id))
            {
                MessageBox.Show("Duplicated requirement found! Please use another item.", "There can't be both!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            LvRequirements.Items.Add(dialog.Result);
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
            if (MessageBox.Show("Are you sure that you want to delete this requirement?", "Make sure you know what you're doing!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            RemoveRq((LvRequirementBinding)LvRequirements.SelectedItem);
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

        private void CopyId(object sender, RoutedEventArgs args)
        {
            var item = LvRequirements.SelectedItem as LvRequirementBinding;
            if (item == null)
                return;
            Clipboard.SetText(item.Id);
        }

    }

}