using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ShipmentSearch
    {
        [JsonPropertyName("shipmentID")]
        public long? ShipmentID { get; set; }
        [JsonPropertyName("shipmentName")]
        public string ShipmentName { get; set; }

        [JsonPropertyName("purchaseOrder")]
        public string PurchaseOrder { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime? OrderDate { get; set; }

        [JsonPropertyName("shipmentDate")]
        public DateTime? ShipmentDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusID")]
        public short? StatusID { get; set; }
    }
}