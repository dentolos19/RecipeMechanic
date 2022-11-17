using System;

namespace RecipeMechanic.Models;

public class IngredientItemModel
{

    public Guid Id { get; init; }
    public string Name { get; set; }
    public int Quantity { get; init; }
    public IngredientDataModel Data { get; init; }

}