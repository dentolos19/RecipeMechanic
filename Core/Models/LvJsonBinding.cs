namespace SmRecipeModifier.Core.Models
{

    public class LvJsonBinding
    {

        public string Name { get; }

        public string Description { get; }

        public string Path { get; }

        public LvJsonBinding(string name, string description, string path)
        {
            Name = name;
            Description = description;
            Path = path;
        }

    }

}