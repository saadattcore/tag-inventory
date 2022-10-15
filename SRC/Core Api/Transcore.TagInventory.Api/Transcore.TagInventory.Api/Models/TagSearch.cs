using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class TagSearch
    {
        [JsonPropertyName("tagID")]
        public string TagID { get; set; }

        [JsonPropertyName("tagNumber")]
        public string TagNumber { get; set; }

        [JsonPropertyName("serialNumber")]
        public long? SerialNumber { get; set; }

        [JsonPropertyName("issuedBoxID")]
        public long? IssuedBoxID { get; set; }

        [JsonPropertyName("receivedBoxID")]
        public long? ReceivedBoxID { get; set; }

        [JsonPropertyName("isImported")]
        public bool? IsImported { get; set; }

        [JsonPropertyName("visualCheckStatusID")]
        public short? VisualCheckStatusID { get; set; }

        [JsonPropertyName("rfidCheckStatusID")]
        public short? RFIDCheckStatusID { get; set; }

        [JsonPropertyName("visualCheckStatus")]
        public string VisualCheckStatus { get; set; }

        [JsonPropertyName("rfidCheckStatus")]
        public string RFIDCheckStatus { get; set; }

        [JsonPropertyName("statusID")]
        public short? StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

    }
}