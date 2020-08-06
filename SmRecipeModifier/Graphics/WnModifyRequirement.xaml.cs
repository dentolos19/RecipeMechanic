using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRequirement
    {

        public SmRequirement RequirementResult { get; private set; }

        public WnModifyRequirement(SmRequirement requirement)
        {
            RequirementResult = requirement;
            InitializeComponent();
        }

    }

}