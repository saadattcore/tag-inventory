using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class ReceivedBoxUpdate
    {
        [JsonPropertyName("receivedBoxID")]
        [Required(ErrorMessage = "Received box id cannot be empty")]
        public long ReceivedBoxID { get; set; }

        [JsonPropertyName("statusID")]
        [Required(ErrorMessage = "Status id required")]
        public short StatusID { get; set; }

        [JsonPropertyName("boxTypeID")]
        [Required(ErrorMessage = "Box type required")]
        public short BoxTypeID { get; set; }

        [JsonPropertyName("updUserID")]
        public int UpdUserID { get; set; }

    }
}