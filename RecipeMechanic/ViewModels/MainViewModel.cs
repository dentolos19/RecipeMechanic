using System.Collections.ObjectModel;
using RecipeMechanic.Models;

namespace RecipeMechanic.ViewModels;

public class MainViewModel : BaseViewModel
{

    private ObservableCollection<RecipeItemModel> _recipeList = new();

    public ObservableCollection<RecipeItemModel> RecipeList
    {
        get => _recipeList;
        set => UpdateProperty(ref _recipeList, value);
    }

}