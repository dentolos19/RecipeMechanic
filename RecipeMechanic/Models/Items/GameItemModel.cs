using System;

namespace RecipeMechanic.Models;

public class GameItemModel
{

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }

}