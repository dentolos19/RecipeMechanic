using System;

namespace SmRecipeModifier.Core.Models
{

    public class SmItem
    {

        public Guid Id { get; }

        public string Name { get; }

        public SmItemInfo Info { get; }

        public SmItem(string id, string name)
        {
            Id = new Guid(id);
            Name = name;
        }

        public SmItem(string id, SmItemInfo info)
        {
            Id = new Guid(id);
            Info = info;
        }

    }

}