namespace SmRecipeModifier.Core.Models
{

    public class SmItem
    {

        public string Name { get; set; }

        public string InGameName { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }
        
        public SmRecipe Recipe { get; set; }

    }

}