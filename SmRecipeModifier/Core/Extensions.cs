using SmRecipeModifier.Core.Models;

namespace SmRecipeModifier.Core
{

    public static class Extensions
    {

        public static SmItem GetItemBasedOnRequirement(this SmItem[] items, SmRequirement requirement)
        {
            foreach (var item in items)
                if (item.Id.Equals(requirement.Id))
                    return item;
            return null;
        }

    }

}