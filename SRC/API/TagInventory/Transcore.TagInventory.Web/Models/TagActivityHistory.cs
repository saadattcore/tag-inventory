using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class TagActivityHistory
    {
        [JsonProperty("dateGroup")]
        public DateTime DateGroup { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}