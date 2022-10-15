using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBoxSearch
    {
        [JsonPropertyName("receivedBoxID")]
        public long? ReceivedBoxID { get; set; }

        [JsonPropertyName("startTag")]
        public long? StartTag { get; set; }

        [JsonPropertyName("endTag")]
        public long? EndTag { get; set; }

        [JsonPropertyName("shipmentID")]
        public long? ShipmentID { get; set; }

        [JsonPropertyName("quantity")]
        public short? Quantity { get; set; }

        [JsonPropertyName("statusID")]
        public short? StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}