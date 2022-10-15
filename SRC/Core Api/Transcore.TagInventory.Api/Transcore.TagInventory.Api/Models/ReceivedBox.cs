using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;
using Transcore.TagInventory.Web.Models;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBox
    {
        [JsonPropertyName("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonPropertyName("startTag")]
        public long StartTag { get; set; }

        [JsonPropertyName("endTag")]
        public long EndTag { get; set; }

        [JsonPropertyName("shipmentID")]
        public long ShipmentID { get; set; }

        [JsonPropertyName("quantity")]
        public short Quantity { get; set; }

        [JsonPropertyName("caseID")]
        public long CaseID { get; set; }

        [JsonPropertyName("statusID")]
        public short StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("boxTypeID")]
        public short BoxTypeID { get; set; }

        [JsonPropertyName("boxType")]
        public string BoxType { get; set; }

        public int CreateUserID { get; set; }

        [JsonPropertyName("updateUserID")]        
        public int UpdateUserID { get; set; }

        [JsonPropertyName("issuedBoxCreated")]
        public bool IssuedBoxCreated { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Remarks { get; set; }

        [JsonPropertyName("boxTags")]
        public List<Tag> Tags { get; set; }

        [JsonPropertyName("shipment")]
        public Shipment Shipment { get; set; }
    }
}