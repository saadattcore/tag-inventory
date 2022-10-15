using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBoxSearch
    {
        [JsonProperty("receivedBoxID")]
        public long? ReceivedBoxID { get; set; }

        [JsonProperty("startTag")]
        public long? StartTag { get; set; }

        [JsonProperty("endTag")]
        public long? EndTag { get; set; }

        [JsonProperty("shipmentID")]
        public long? ShipmentID { get; set; }

        [JsonProperty("quantity")]
        public short? Quantity { get; set; }

        [JsonProperty("statusID")]
        public short? StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}