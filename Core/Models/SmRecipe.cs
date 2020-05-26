using System;
using Newtonsoft.Json;

namespace SmRecipeModifier.Core.Models
{

    public class SmRecipe
    {

        [JsonProperty("itemId")]
        public Guid Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("craftTime")]
        public int Duration { get; set; }

        [JsonProperty("ingredientList")]
        public SmRequirement[] Requirements { get; set; }

    }

}