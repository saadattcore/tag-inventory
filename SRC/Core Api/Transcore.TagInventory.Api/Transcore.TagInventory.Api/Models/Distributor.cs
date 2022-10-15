using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Distributor
    {
        [JsonPropertyName("distributorID")]
        public short DistributorID { get; set; }

        [JsonPropertyName("distributorName")]
        public string DistributorName { get; set; }

        [JsonPropertyName("paymentLocationTypeID")]
        public byte PaymentLocationTypeID { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("distributorTypeId")]
        public short DistributorTypeId { get; set; }

        [JsonPropertyName("distributorTypeName")]
        public string DistributorTypeName { get; set; }

        [JsonPropertyName("distributorTypeDesc")]
        public string DistributorTypeDesc { get; set; }
    }

    public class DistributorType
    {
        [JsonPropertyName("distributorTypeID")]
        public short DistributorTypeID { get; set; }

        [JsonPropertyName("distributorTypeName")]
        public string DistributorTypeName { get; set; }

        [JsonProperty("distributorTypeDesc")]
        public string DistributorTypeDesc { get; set; }


    }

    public class DistributorAndTypes
    {
        [JsonPropertyName("distributors")]
        public List<Distributor> Distributors { get; set; }

        [JsonPropertyName("distributorTypes")]
        public List<DistributorType> DistributorTypes { get; set; }
    }
}