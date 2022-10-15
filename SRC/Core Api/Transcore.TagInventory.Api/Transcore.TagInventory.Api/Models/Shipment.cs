using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Shipment 
    {
        [JsonPropertyName("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonPropertyName("shipmentName")]
        public string ShipmentName { get; set; }

        [Required]
        [JsonPropertyName("purchaseOrder")]
        public string PurchaseOrder { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("shipmentDate")]
        public DateTime ShipmentDate { get; set; }

        [JsonPropertyName("statusID")]
        public short StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        public int CreatedUserID { get; set; }

        [JsonPropertyName("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonIgnore]
        public List<ReceivedBox> ReceivedBoxes { get; set; }    

        public int UdpatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}