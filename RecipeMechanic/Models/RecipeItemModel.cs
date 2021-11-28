using System;

namespace RecipeMechanic.Models;

public record RecipeItemModel
{

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RecipeDataModel Recipe { get; set; }

}