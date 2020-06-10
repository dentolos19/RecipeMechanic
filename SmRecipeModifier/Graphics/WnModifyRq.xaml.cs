using System.Windows;
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRq
    {

        public WnModifyRq(LvRequirementBinding binding)
        {
            InitializeComponent();
            LbTitle.Content = binding.Name;
            TbQuantity.Text = binding.Quantity.ToString();
            foreach (var item in App.WindowMain.ItemDictionary.Items)
                CbItem.Items.Add(new ComboBoxItem { Content = item.Name, Tag = item.Id });
            CbItem.Text = binding.Name;
        }

        public LvRequirementBinding Result { get; private set; }

        private void Cancel(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void Save(object sender, RoutedEventArgs args)
        {
            var item = CbItem.SelectedItem as ComboBoxItem;
            Result = new LvRequirementBinding(int.Parse(TbQuantity.Text), item?.Content.ToString(), item?.Tag.ToString());
            DialogResult = true;
            Close();
        }

    }

}