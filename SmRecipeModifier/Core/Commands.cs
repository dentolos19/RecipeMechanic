using System.Windows.Input;

namespace SmRecipeModifier.Core
{

    public static class Commands
    {

        public static readonly RoutedUICommand AddRecipe = new("Add Recipe", "Add Recipe", typeof(Commands));
        public static readonly RoutedUICommand RemoveRecipe = new("Remove Recipe", "Remove Recipe", typeof(Commands));
        public static readonly RoutedUICommand ModifyRecipe = new("Modify Recipe", "Modify Recipe", typeof(Commands));
        public static readonly RoutedUICommand ModifyAllRecipe = new("Modify All Recipe", "Modify All Recipe", typeof(Commands));

    }

}