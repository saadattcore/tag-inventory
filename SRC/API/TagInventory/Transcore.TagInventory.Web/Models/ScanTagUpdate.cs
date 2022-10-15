using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class BoxScanTagUpdate
    {
        [JsonProperty("receivedBoxID")]
        [Required]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("statusID")]
        [Required]
        public short StatusID { get; set; }

        [JsonProperty("updUserID")]
        [Required]
        public int UpdUserID { get; set; }

        [JsonProperty("scanTags")]
        public List<TagStatusUpdate> ScanTags { get; set; }
    }
}