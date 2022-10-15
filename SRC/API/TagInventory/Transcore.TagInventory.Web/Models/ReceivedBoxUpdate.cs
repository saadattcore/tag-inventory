using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBoxUpdate
    {
        [JsonProperty("receivedBoxID")]
        public long ReceivedBoxID { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("boxTypeID")]
        public short BoxTypeID { get; set; }

        [JsonProperty("updUserID")]
        public int UpdUserID { get; set; }

    }
}