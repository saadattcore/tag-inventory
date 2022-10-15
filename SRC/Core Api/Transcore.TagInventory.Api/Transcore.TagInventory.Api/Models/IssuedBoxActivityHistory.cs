using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBoxActivityHistory
    {
        [JsonPropertyName("dateGroup")]
        public DateTime DateGroup { get; set; }

        [JsonPropertyName("issuedBoxList")]
        public List<IssuedBox> IssuedBoxList { get; set; }
    }
}