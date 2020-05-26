using System.IO;
using System.Windows;

namespace SmRecipeModifier.Graphics
{

    public partial class WnMain
    {

        private string _path;

        public WnMain()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new WnOpen { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _path = dialog.FileName;
            TbFileName.Text = Path.GetFileName(_path)!;
            BnSave.IsEnabled = true;
            BnSaveAs.IsEnabled = true;
            MiSave.IsEnabled = true;
            MiSaveAs.IsEnabled = true;
            BnAddRecipe.IsEnabled = true;
        }

        private void Save(object sender, RoutedEventArgs args)
        {
            if (BnSave.IsEnabled == false)
                return;
            // todo
        }

        private void SaveAs(object sender, RoutedEventArgs args)
        {
            if (BnSave.IsEnabled == false)
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