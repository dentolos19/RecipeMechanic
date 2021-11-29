using RecipeMechanic.Models;
using System;
using System.Linq;
using System.Windows;

namespace RecipeMechanic.Views;

public partial class ManageIngredientWindow
{

    private GameItemModel[] _items;

    public IngredientDataModel? Ingredient { get; private set; }

    public ManageIngredientWindow(GameItemModel[] items, IngredientDataModel? ingredient = null)
    {
        _items = items;
        Ingredient = ingredient;
        InitializeComponent();
    }

    private void OnInitialized(object? sender, EventArgs args)
    {
        foreach (var item in _items)
            ItemSelection.Items.Add(item);
        if (Ingredient is not null)
        {
            ItemSelection.SelectedItem = _items.FirstOrDefault(item => item.Id.Equals(Ingredient.Id));
            QuantityInput.Value = Ingredient.Quantity;
        }
        if (ItemSelection.SelectedItem is null)
            ItemSelection.SelectedIndex = new Random().Next(ItemSelection.Items.Count - 1);
    }

    private void OnContinue(object sender, RoutedEventArgs args)
    {
        if (ItemSelection.SelectedItem is not GameItemModel item)
        {
            MessageBox.Show("You must select a valid ingredient item for the recipe!", "Recipe Mechanic");
            return;
        }
        Ingredient = new IngredientDataModel
        {
            Id = item.Id,
            Quantity = (int)QuantityInput.Value
        };
        DialogResult = true;
        Close();
    }

}