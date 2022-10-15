using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Tag
    {
        [JsonProperty("tagID")]
        [Required]
        public string TagID { get; set; }
        
        [JsonProperty("tagNumber")]
        [Required]
        public string TagNumber { get; set; }

        [JsonProperty("serialNumber")]
        [Required]
        public long SerialNumber { get; set; }

        [JsonProperty("issuedBoxID")]
        public long? IssuedBoxID { get; set; }

        [JsonProperty("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("isImported")]
        public bool IsImported { get; set; }

        [JsonProperty("visualCheckStatusID")]
        public short VisualCheckStatusID { get; set; }

        [JsonProperty("visualCheckStatus")]
        public string VisualCheckStatus { get; set; }   

        [JsonProperty("rfidCheckStatusID")]
        public short RFIDCheckStatusID { get; set; }

        [JsonProperty("rfidCheckStatus")]
        public string RFIDCheckStatus { get; set; }

        [JsonProperty("kitVisualCheckStatusID")]
        public short KitVisualCheckStatusID { get; set; }

        [JsonProperty("kitVisualCheckStatus")]
        public string KitVisualCheckStatus { get; set; }

        [JsonProperty("kitRFIDCheckStatusID")]
        public short KitRFIDCheckStatusID { get; set; }

        [JsonProperty("kitRFIDCheckStatus")]
        public string KitRFIDCheckStatus { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tagTypeID")]
        public short TagTypeID { get; set; }

        [JsonProperty("tagType")]
        public string TagType { get; set; }

        [JsonProperty("rejectionReason")]
        public string RejectionReason { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("pIN")]
        public int PIN { get; set; }

        [JsonProperty("receivedBox")]
        public ReceivedBox ReceivedBox { get; set; }
    }
}