using System.Windows;

namespace SmRecipeModifier.Graphics
{

    public partial class WnMain
    {

        public WnMain()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void Save(object sender, RoutedEventArgs args)
        {
            if (TbFileName.Text.Length ! > 0)
                return;
            // todo
        }

        private void SaveAs(object sender, RoutedEventArgs args)
        {
            if (TbFileName.Text.Length! > 0)
                return;
            // todo
        }

        private void AddRecipe(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void RemoveRecipe(object sender, RoutedEventArgs args)
        {
            // todo
        }

        private void ModifyRecipe(object sender, RoutedEventArgs args)
        {
            // todo
        }

    }

}