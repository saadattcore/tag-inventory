using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ShipmentCreateUpdate
    {
        [JsonProperty("shipmentID")]
        public long  ShipmentID { get; set; }

        [JsonProperty("shipmentName")]
        [Required]
        public string ShipmentName { get; set; }

        [Required]
        [JsonProperty("purchaseOrder")]
        public string PurchaseOrder { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("shipmentDate")]
        public DateTime ShipmentDate { get; set; }

        [JsonProperty("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdUserID")]
        public int CreatedUserID { get; set; }
    }
}