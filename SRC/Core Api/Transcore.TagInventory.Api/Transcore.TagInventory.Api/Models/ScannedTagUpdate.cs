using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ScannedTagUpdate
    {
        [JsonPropertyName("tagID")]
        public long TagID { get; set; }

        [JsonPropertyName("tagNumber")]
        public string TagNumber { get; set; }

        [JsonPropertyName("serialNumber")]
        public long SerialNumber { get; set; }

        [JsonPropertyName("pin")]
        public int PIN { get; set; }

        [JsonPropertyName("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonPropertyName("isImported")]
        public bool IsImported { get; set; }

        [JsonPropertyName("statusID")]
        public short StatusID { get; set; }

        [JsonPropertyName("visualCheckStatusID")]
        public short VisualCheckStatusID { get; set; }

        [JsonPropertyName("rfidCheckStatusID")]
        public short RFIDCheckStatusID { get; set; }

        [JsonPropertyName("createdUserID")]
        public int CreatedUserID { get; set; }

        [JsonPropertyName("updateUserID")]
        public int UpdatedUserID { get; set; }
    }
}