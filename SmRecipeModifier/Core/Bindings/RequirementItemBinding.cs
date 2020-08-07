using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core.Bindings
{

    public class RequirementItemBinding
    {

        public RequirementItemBinding(string name, int quantity, string id)
        {
            Name = name;
            Quantity = quantity;
            Id = id;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Id { get; set; }

        public static RequirementItemBinding ConvertFromRequirement(SmRequirement requirement)
        {
            var item = App.AvailableItems.GetItemBasedOnRequirement(requirement);
            return new RequirementItemBinding(item.InGameName ?? item.Name, requirement.Quantity, item.Id);
        }

    }

}