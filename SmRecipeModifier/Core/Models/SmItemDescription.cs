using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{

    public class SmItemDescription
    {

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }

}