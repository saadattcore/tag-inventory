using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Shipment 
    {
        [JsonProperty("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonProperty("shipmentName")]
        public string ShipmentName { get; set; }

        [Required]
        [JsonProperty("purchaseOrder")]
        public string PurchaseOrder { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("shipmentDate")]
        public DateTime ShipmentDate { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public int CreatedUserID { get; set; }

        [JsonProperty("deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [JsonIgnore]
        public List<ReceivedBox> ReceivedBoxes { get; set; }    

        public int UdpatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}