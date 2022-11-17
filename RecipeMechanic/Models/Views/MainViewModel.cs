using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RecipeMechanic.Models;

public partial class MainViewModel : ObservableObject
{

    [ObservableProperty] private string _openedFileText;
    [ObservableProperty] private string _itemCountText;
    [ObservableProperty] private string _recipeCountText;
    [ObservableProperty] private string _appVersionText;
    [ObservableProperty] private ObservableCollection<RecipeItemModel> _recipes = new();
    [ObservableProperty] private ObservableCollection<GameItemModel> _items = new();

}