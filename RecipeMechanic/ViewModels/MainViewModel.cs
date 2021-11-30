using RecipeMechanic.Models;
using System.Collections.ObjectModel;

namespace RecipeMechanic.ViewModels;

public class MainViewModel : BaseViewModel
{

    private string _openedFileText;
    private string _itemCountText;
    private string _recipeCountText;
    private string _appVersionText;
    private ObservableCollection<RecipeItemModel> _recipeList = new();
    private ObservableCollection<GameItemModel> _itemList = new();

    public string OpenedFileText
    {
        get => _openedFileText;
        set => UpdateProperty(ref _openedFileText, value);
    }

    public string ItemCountText
    {
        get => _itemCountText;
        set => UpdateProperty(ref _itemCountText, value);
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

    public ObservableCollection<GameItemModel> ItemList
    {
        get => _itemList;
        set => UpdateProperty(ref _itemList, value);
    }

}