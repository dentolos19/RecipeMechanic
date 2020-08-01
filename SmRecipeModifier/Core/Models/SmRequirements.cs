using System.Text.Json.Serialization;

namespace SmRecipeModifier.Core.Models
{

    public class SmRequirements
    {

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("itemId")]
        public string Id { get; set; }

    }

}