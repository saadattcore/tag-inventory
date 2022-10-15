using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Tag
    {
        [JsonPropertyName("tagID")]
        [Required(ErrorMessage = "Tag is is required")]
        public string TagID { get; set; }

        [JsonPropertyName("tagNumber")]
        [Required(ErrorMessage = "Tag number is required")]
        public string TagNumber { get; set; }

        [JsonPropertyName("serialNumber")]
        [Required(ErrorMessage = "Serial number is required")]
        public long SerialNumber { get; set; }

        [JsonPropertyName("issuedBoxID")]
        public long? IssuedBoxID { get; set; }

        [JsonPropertyName("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonPropertyName("isImported")]
        public bool IsImported { get; set; }

        [JsonPropertyName("visualCheckStatusID")]
        public short VisualCheckStatusID { get; set; }

        [JsonPropertyName("visualCheckStatus")]
        public string VisualCheckStatus { get; set; }

        [JsonPropertyName("rfidCheckStatusID")]
        public short RFIDCheckStatusID { get; set; }

        [JsonPropertyName("rfidCheckStatus")]
        public string RFIDCheckStatus { get; set; }

        [JsonPropertyName("kitVisualCheckStatusID")]
        public short KitVisualCheckStatusID { get; set; }

        [JsonPropertyName("kitVisualCheckStatus")]
        public string KitVisualCheckStatus { get; set; }

        [JsonPropertyName("kitRFIDCheckStatusID")]
        public short KitRFIDCheckStatusID { get; set; }

        [JsonPropertyName("kitRFIDCheckStatus")]
        public string KitRFIDCheckStatus { get; set; }

        [JsonPropertyName("statusID")]
        public short StatusID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("tagTypeID")]
        public short TagTypeID { get; set; }

        [JsonPropertyName("tagType")]
        public string TagType { get; set; }

        [JsonPropertyName("rejectionReason")]
        public string RejectionReason { get; set; }

        [JsonPropertyName("createdBy")]
        public int CreatedBy { get; set; }

        [JsonPropertyName("updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonPropertyName("remarks")]
        public string Remarks { get; set; }

        [JsonPropertyName("pIN")]
        public int PIN { get; set; }

        [JsonPropertyName("receivedBox")]
        public ReceivedBox ReceivedBox { get; set; }
    }
}