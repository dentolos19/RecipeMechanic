using System;

namespace RecipeMechanic.Models;

public class IngredientItemModel
{

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }

}