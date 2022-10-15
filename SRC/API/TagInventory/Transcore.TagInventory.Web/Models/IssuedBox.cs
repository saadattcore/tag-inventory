using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBox
    {
        [JsonProperty("issuedBoxID")]
        public long IssuedBoxID { get; set; }

        [JsonProperty("receivedBoxID")]        
        public long ReceivedBoxID { get; set; }

        [JsonProperty("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonProperty("quantity")]
        public short Quantity { get; set; }

        [JsonProperty("statusID")]
        public long StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("sendDate")]
        public DateTime? SendDate { get; set; }

        [JsonProperty("receivedDate")]
        public DateTime? ReceivedDate { get; set; }

        [JsonProperty("distributorID")]
        public short? DistributorID { get; set; }

        [JsonProperty("distributor")]
        public string Distributor { get; set; }

        [JsonProperty("issuedDate")]
        public DateTime? IssuedDate { get; set; }

        [JsonProperty("initialAssigned")]
        public bool InitialAssigned { get; set; }

        [JsonProperty("updateUserID")]
        public int UpdateUserID { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updatedDatePart")]
        public DateTime UpdatedDatePart { get; set; }


        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}