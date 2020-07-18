namespace SmRecipeModifier.Core.Models
{

    public class LvItemBinding
    {

        public LvItemBinding(string name, string inGameName, string id)
        {
            Id = id;
            Name = name;
            InGameName = inGameName;
        }

        public string Id { get; }

        public string Name { get; }

        public string InGameName { get; }

    }

}