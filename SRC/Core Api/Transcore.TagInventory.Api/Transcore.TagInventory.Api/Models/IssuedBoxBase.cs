using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBoxBase
    {
        [Required]
        [JsonProperty("quantity")]
        public short Quantity { get; set; }

        [JsonProperty("statusID")]
        public short StatusID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("receivedBoxID")]
        public string ReceivedBoxID { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
       
        public int CreatedUserID { get; set; }
    }
}