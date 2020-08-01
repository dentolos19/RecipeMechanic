using System.Windows;

namespace SmRecipeModifier.Graphics
{

    public partial class WnOpen
    {

        public WnOpen()
        {
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs args)
        {
            // TODO
            DialogResult = true;
            Close();
        }

    }

}