using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Forge.Shared.ViewModels
{
    public class BaseFiltered<TModel>
    {
        [JsonPropertyName("models")]
        public List<TModel> Models { get; set; }

        [JsonPropertyName("filtered")]
        public int Filtered { get; set; }
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
