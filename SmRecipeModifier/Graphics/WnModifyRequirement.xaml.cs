using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Graphics
{

    public partial class WnModifyRequirement
    {

        public SmRequirement RequirementResult { get; }

        public WnModifyRequirement(SmRequirement requirement)
        {
            RequirementResult = requirement;
            InitializeComponent();
        }

    }

}