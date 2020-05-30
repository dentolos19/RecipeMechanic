namespace SmRecipeModifier.Core.Models
{
    public class SmItem
    {
        public SmItem(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public SmItem(string id, SmItemInfo info)
        {
            Id = id;
            Info = info;
        }

        public string Id { get; }

        public string Name { get; }

        public SmItemInfo Info { get; }
    }
}