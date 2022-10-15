using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ScannedReceivedBoxUpdate
    {
        [JsonPropertyName("receivedBoxID")]
        [Required(ErrorMessage = "Received box id is required")]
        public long ReceivedBoxID { get; set; }

        [JsonPropertyName("receivedBoxStatus")]
        [Required(ErrorMessage = "Status id is required ")]
        public short StatusID { get; set; }

        [JsonPropertyName("updateUserID")]
        public int UpdateUserID { get; set; }

        [JsonPropertyName("scanTags")]
        public List<ScannedTagUpdate> ScanTags { get; set; }
    }
}