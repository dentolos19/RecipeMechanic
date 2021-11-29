using System.Windows.Input;

namespace RecipeMechanic.Core;

public static class Commands
{

    public static readonly RoutedUICommand AddRecipe = new("Add Recipe", "Add Recipe", typeof(Commands));
    public static readonly RoutedUICommand ModifyRecipe = new("Modify Recipe", "Modify Recipe", typeof(Commands));
    // public static readonly RoutedUICommand ModifyAllRecipes = new("Modify All Recipes", "Modify All Recipes", typeof(Commands));

}