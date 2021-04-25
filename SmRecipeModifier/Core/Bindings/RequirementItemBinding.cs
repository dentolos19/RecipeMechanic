using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core.Bindings
{

    public class RequirementItemBinding
    {

        public RequirementItemBinding(SmRequirement requirement)
        {
            var item = App.AvailableItems.GetItemBasedOnRequirement(requirement);
            Name = item.Name;
            Quantity = requirement.Quantity;
            Id = requirement.Id;
            Requirement = requirement;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Id { get; set; }
        public SmRequirement Requirement { get; set; }

    }

}