using System.Collections.ObjectModel;
using RecipeMechanic.Models;

namespace RecipeMechanic.ViewModels;

public class MainViewModel : BaseViewModel
{

    private string _openedFilePath;
    private string _recipeCountText;
    private string _appVersionText;
    private ObservableCollection<RecipeItemModel> _recipeList = new();

    public string OpenedFilePath
    {
        get => _openedFilePath;
        set => UpdateProperty(ref _openedFilePath, value);
    }

    public string RecipeCountText
    {
        get => _recipeCountText;
        set => UpdateProperty(ref _recipeCountText, value);
    }

    public string AppVersionText
    {
        get => _appVersionText;
        set => UpdateProperty(ref _appVersionText, value);
    }

    public ObservableCollection<RecipeItemModel> RecipeList
    {
        get => _recipeList;
        set => UpdateProperty(ref _recipeList, value);
    }

}