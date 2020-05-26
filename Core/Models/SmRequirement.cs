using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{

    public class SmRequirement
    {

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("itemId")]
        public string Id { get; set; }

    }

}