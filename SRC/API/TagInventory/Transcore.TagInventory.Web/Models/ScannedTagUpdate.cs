using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ScannedTagUpdate
    {
        [JsonProperty("tagID")]
        public long TagID { get; set; }

        [JsonProperty("tagNumber")]
        public string TagNumber { get; set; }

        [JsonProperty("serialNumber")]
        public long SerialNumber { get; set; }

        [JsonProperty("pin")]
        public int PIN { get; set; }

        [JsonProperty("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("isImported")]
        public bool IsImported { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("visualCheckStatusID")]
        public short VisualCheckStatusID { get; set; }

        [JsonProperty("rfidCheckStatusID")]
        public short RFIDCheckStatusID { get; set; }

        [JsonProperty("createdUserID")]
        public int CreatedUserID { get; set; }

        [JsonProperty("updateUserID")]
        public int UpdatedUserID { get; set; }
    }
}