using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class DataTableColumnModel
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "Column";

        [JsonProperty("autoWidth")]
        public bool AutoWidth { get; set; } = true;
    }
}
