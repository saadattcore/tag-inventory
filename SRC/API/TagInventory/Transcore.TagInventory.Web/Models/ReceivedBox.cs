using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Transcore.TagInventory.Web.Models;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBox
    {
        [JsonProperty("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("startTag")]
        public long StartTag { get; set; }

        [JsonProperty("endTag")]
        public long EndTag { get; set; }

        [JsonProperty("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonProperty("quantity")]
        public short Quantity { get; set; }

        [JsonProperty("caseID")]
        public long CaseID { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("boxTypeID")]
        public short BoxTypeID { get; set; }

        [JsonProperty("boxType")]
        public string BoxType { get; set; }

        public int CreateUserID { get; set; }

        [JsonProperty("updateUserID")]        
        public int UpdateUserID { get; set; }

        [JsonProperty("issuedBoxCreated")]
        public bool IssuedBoxCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Remarks { get; set; }

        [JsonProperty("boxTags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("shipment")]
        public Shipment Shipment { get; set; }
    }
}