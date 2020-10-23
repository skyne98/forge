using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class DataTableRequest
    {
        [JsonProperty("draw")]
        public int? Draw { get; set; }

        [JsonProperty("start")]
        public int? Skip { get; set; }

        [JsonProperty("length")]
        public int? Take { get; set; }

        [JsonProperty("order")]
        public List<DataTableRequestOrder> Orderings { get; set; }
    }

    public class DataTableRequestOrder
    {
        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
}
