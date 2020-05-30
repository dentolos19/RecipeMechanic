using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{
    public class SmItemInfo
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public string Description { get; set; }
    }
}