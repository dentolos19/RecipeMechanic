using System;

namespace RecipeMechanic.Models;

public class RecipeItemModel
{

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RecipeDataModel Data { get; init; }

}