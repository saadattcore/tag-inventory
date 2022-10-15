using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class TagSearch
    {
        [JsonProperty("tagID")]
        public string TagID { get; set; }

        [JsonProperty("tagNumber")]
        public string TagNumber { get; set; }

        [JsonProperty("serialNumber")]
        public long? SerialNumber { get; set; }

        [JsonProperty("issuedBoxID")]
        public long? IssuedBoxID { get; set; }

        [JsonProperty("receivedBoxID")]
        public long? ReceivedBoxID { get; set; }

        [JsonProperty("isImported")]
        public bool? IsImported { get; set; }

        [JsonProperty("visualCheckStatusID")]
        public short? VisualCheckStatusID { get; set; }

        [JsonProperty("rfidCheckStatusID")]
        public short? RFIDCheckStatusID { get; set; }

        [JsonProperty("visualCheckStatus")]
        public string VisualCheckStatus { get; set; }

        [JsonProperty("rfidCheckStatus")]
        public string RFIDCheckStatus { get; set; }

        [JsonProperty("statusID")]
        public short? StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

    }
}