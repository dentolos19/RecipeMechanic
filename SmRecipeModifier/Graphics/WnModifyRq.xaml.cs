using System.Windows;
<<<<<<< Updated upstream:SmRecipeModifier/Graphics/WnModifyRq.xaml.cs
using System.Windows.Controls;
using SmRecipeModifier.Core.Models;
=======
using Ookii.Dialogs.Wpf;
>>>>>>> Stashed changes:SmRecipeModifier/Graphics/WnIntro.xaml.cs

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRq
    {

        public WnModifyRq(LvRequirementBinding binding, bool addNew = false)
        {
            InitializeComponent();
            LbTitle.Content = addNew ? "New Recipe Requirement" : binding.Name;
            TbQuantity.Text = binding.Quantity.ToString();
            foreach (var item in App.WindowMain.ItemDictionary.Items)
                CbItem.Items.Add(new ComboBoxItem { Content = item.Name, Tag = item.Id });
            CbItem.Text = binding.Name;
        }

        public LvRequirementBinding Result { get; private set; }

        private void Cancel(object sender, RoutedEventArgs args)
        {
<<<<<<< Updated upstream:SmRecipeModifier/Graphics/WnModifyRq.xaml.cs
            Close();
=======
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this) == true)
                GameDataPathBox.Text = dialog.SelectedPath;
>>>>>>> Stashed changes:SmRecipeModifier/Graphics/WnIntro.xaml.cs
        }

        private void Save(object sender, RoutedEventArgs args)
        {
<<<<<<< Updated upstream:SmRecipeModifier/Graphics/WnModifyRq.xaml.cs
            var item = CbItem.SelectedItem as ComboBoxItem;
            Result = new LvRequirementBinding(int.Parse(TbQuantity.Text), item?.Content.ToString(), item?.Tag.ToString());
            DialogResult = true;
=======
            if (string.IsNullOrEmpty(GameDataPathBox.Text))
            {
                MessageBox.Show("You need to enter the game's data path before continuing!", "SmRecipeModifier");
                return;
            }
            App.Settings.GameDataPath = GameDataPathBox.Text;
            App.Settings.Save();
>>>>>>> Stashed changes:SmRecipeModifier/Graphics/WnIntro.xaml.cs
            Close();
        }

    }

}