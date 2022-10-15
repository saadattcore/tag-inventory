using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ShipmentCreateUpdate
    {
        [JsonPropertyName("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonPropertyName("shipmentName")]
        [Required(ErrorMessage = "Shipment name is required")]
        public string ShipmentName { get; set; }

        [JsonPropertyName("purchaseOrder")]
        [Required(ErrorMessage = "Purchase order is required")]
        public string PurchaseOrder { get; set; }


        [JsonPropertyName("orderDate")]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yy")]
        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime? OrderDate { get; set; }


        [JsonPropertyName("shipmentDate")]
        [Required(ErrorMessage = "ShipmentDate is required")]
        public DateTime? ShipmentDate { get; set; }


        [JsonPropertyName("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonPropertyName("statusID")]
        public short StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("createdUserID")]
        public int CreatedUserID { get; set; }
    }
}