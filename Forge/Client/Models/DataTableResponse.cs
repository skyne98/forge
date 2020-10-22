using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class DataTableResponse
    {
        [JsonProperty("draw")]
        public int? Draw { get; set; }

        [JsonProperty("recordsFiltered")]
        public int Filtered { get; set; }

        [JsonProperty("recordsTotal")]
        public int Total { get; set; }

        [JsonProperty("data")]
        public List<JObject> Data { get; set; }
    }
}
