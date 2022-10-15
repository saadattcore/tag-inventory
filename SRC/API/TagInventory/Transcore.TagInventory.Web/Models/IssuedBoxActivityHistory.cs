using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBoxActivityHistory
    {
        [JsonProperty("dateGroup")]
        public DateTime DateGroup { get; set; }
        [JsonProperty("issuedBoxList")]
        public List<IssuedBox> IssuedBoxList { get; set; }
    }
}