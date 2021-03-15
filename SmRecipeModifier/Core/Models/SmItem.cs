using System.Collections.Generic;

namespace SmRecipeModifier.Core.Models
{

    public class SmItem
    {

        public string Name { get; set; }

        public string InGameName { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public SmRecipe Recipe { get; set; }

        public class Comparer : IEqualityComparer<SmItem>
        {

            public bool Equals(SmItem? left, SmItem? right)
            {
                return left?.Id == right?.Id;
            }

            public int GetHashCode(SmItem @object)
            {
                return @object.GetHashCode();
            }

        }

    }

}