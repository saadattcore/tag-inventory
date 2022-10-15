using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Page<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("searchCount")]
        public int SearchCount { get; set; }
    }
}