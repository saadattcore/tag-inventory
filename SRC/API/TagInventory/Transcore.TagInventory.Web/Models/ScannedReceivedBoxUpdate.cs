using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ScannedReceivedBoxUpdate
    {
        [JsonProperty("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("receivedBoxStatus")]
        public short StatusID { get; set; }

        [JsonProperty("updateUserID")]
        public int UpdateUserID { get; set; }

        [JsonProperty("scanTags")]
        public List<ScannedTagUpdate> ScanTags { get; set; }
    }
}