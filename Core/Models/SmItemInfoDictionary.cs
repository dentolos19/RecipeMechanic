using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{

    public class SmItemInfoDictionary
    {

        private readonly string _path;

        public SmItem[] Items { get; private set; }

        public SmItemInfoDictionary(string path)
        {
            _path = path;
            Refresh();
        }

        public void Refresh()
        {
            var data = File.ReadAllText(_path);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, SmItemInfo>>(data);
            var list = new List<SmItem>();
            foreach (var (id, info) in dictionary)
                list.Add(new SmItem(id, info));
            Items = list.ToArray();
        }

        public SmItem Fetch(Guid id)
        {
            foreach (var item in Items)
                if (item.Id.ToString() == id.ToString())
                    return item;
            return null;
        }

    }

}