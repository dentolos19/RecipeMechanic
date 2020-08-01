using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{

    public class SmRequirements
    {

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("itemId")]
        public string Id { get; set; }

    }

}