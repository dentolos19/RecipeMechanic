using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRecipe
    {

        public SmRecipe RecipeResult { get; private set; }

        public WnModifyRecipe(SmRecipe recipe)
        {
            RecipeResult = recipe;
            InitializeComponent();
        }

    }

}