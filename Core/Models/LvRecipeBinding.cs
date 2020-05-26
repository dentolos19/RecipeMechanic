namespace SmRecipeModifier.Core.Models
{

    public class LvRecipeBinding
    {

        public string Name { get; }

        public string Id { get; }

        public LvRecipeBinding(string name, string id)
        {
            Name = name;
            Id = id;
        }

    }

}