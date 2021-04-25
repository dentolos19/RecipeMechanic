namespace SmRecipeModifier.Core.Bindings
{

    public class JsonItemBinding
    {

        public JsonItemBinding(string name, string description, string path)
        {
            Name = name;
            Description = description;
            Path = path;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

    }

}