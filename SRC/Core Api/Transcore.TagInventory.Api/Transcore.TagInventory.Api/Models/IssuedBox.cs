using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBox
    {
        [JsonPropertyName("issuedBoxID")]
        public long IssuedBoxID { get; set; }

        [Required(ErrorMessage = "Received box id is required")]
        [JsonPropertyName("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [Required(ErrorMessage = "Shipment Id is required")]
        [JsonPropertyName("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonPropertyName("quantity")]
        public short Quantity { get; set; }


        [JsonPropertyName("statusID")]
        public long StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("sendDate")]
        public DateTime? SendDate { get; set; }

        [JsonPropertyName("receivedDate")]
        public DateTime? ReceivedDate { get; set; }

        [JsonPropertyName("distributorID")]
        public short? DistributorID { get; set; }

        [JsonPropertyName("distributor")]
        public string Distributor { get; set; }

        [JsonPropertyName("issuedDate")]
        public DateTime? IssuedDate { get; set; }

        [JsonPropertyName("initialAssigned")]
        public bool InitialAssigned { get; set; }

        [JsonPropertyName("updateUserID")]
        public int UpdateUserID { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonPropertyName("updatedDatePart")]
        public DateTime UpdatedDatePart { get; set; }

        [JsonPropertyName("remarks")]
        public string Remarks { get; set; }

        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }
    }
}