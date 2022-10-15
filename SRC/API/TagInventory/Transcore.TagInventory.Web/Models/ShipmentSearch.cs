using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ShipmentSearch
    {
        [JsonProperty("shipmentID")]
        public long? ShipmentID { get; set; }
        [JsonProperty("shipmentName")]
        public string ShipmentName { get; set; }

        [JsonProperty("purchaseOrder")]
        public string PurchaseOrder { get; set; }

        [JsonProperty("orderDate")]
        public DateTime? OrderDate { get; set; }

        [JsonProperty("shipmentDate")]
        public DateTime? ShipmentDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusID")]
        public short? StatusID { get; set; }
    }
}