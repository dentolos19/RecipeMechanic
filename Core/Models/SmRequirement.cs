using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{
    public class SmRequirement
    {
        public SmRequirement(int quantity, string id)
        {
            Quantity = quantity;
            Id = id;
        }

        [JsonProperty("quantity")] public int Quantity { get; set; }

        [JsonProperty("itemId")] public string Id { get; set; }
    }
}