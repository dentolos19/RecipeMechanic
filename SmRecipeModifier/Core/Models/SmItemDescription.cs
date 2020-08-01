using System.Text.Json.Serialization;

namespace SmRecipeModifier.Core.Models
{

    public class SmItemDescription
    {

        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }

}