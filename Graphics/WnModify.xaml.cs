using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModify
    {

        public SmRecipe Input { private get; set; }
        public SmRecipe Result { get; private set; }

        public WnModify()
        {
            InitializeComponent();
        }

    }

}