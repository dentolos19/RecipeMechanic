using System.Text.Json.Serialization;

namespace SmRecipeModifier.Core.Models
{

    public class SmRecipe
    {

        [JsonPropertyName("itemId")]
        public string Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("craftTime")]
        public int Duration { get; set; }

        [JsonPropertyName("ingredientList")]
        public SmRequirements[] Requirements { get; set; }

    }

}