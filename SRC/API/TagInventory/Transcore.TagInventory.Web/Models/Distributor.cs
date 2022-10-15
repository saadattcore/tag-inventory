using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class Distributor
    {
        [JsonProperty("distributorID")]
        public short DistributorID { get; set; }

        [JsonProperty("distributorName")]
        public string DistributorName { get; set; }

        [JsonProperty("paymentLocationTypeID")]
        public byte PaymentLocationTypeID { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("distributorTypeId")]
        public short DistributorTypeId { get; set; }

        [JsonProperty("distributorTypeName")]
        public string DistributorTypeName { get; set; }

        [JsonProperty("distributorTypeDesc")]
        public string DistributorTypeDesc { get; set; }
    }

    public class DistributorType
    {
        [JsonProperty("distributorTypeID")]
        public short DistributorTypeID { get; set; }

        [JsonProperty("distributorTypeName")]
        public string DistributorTypeName { get; set; }

        [JsonProperty("distributorTypeDesc")]
        public string DistributorTypeDesc { get; set; }


    }

    public class DistributorAndTypes
    {
        [JsonProperty("distributors")]
        public List<Distributor> Distributors { get; set; }

        [JsonProperty("distributorTypes")]
        public List<DistributorType> DistributorTypes { get; set; }
    }
}