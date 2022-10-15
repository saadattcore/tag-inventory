using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Core
{
    public class Distributor
    {
        public short DistributorID { get; set; }
        public string DistributorName { get; set; }
        public byte PaymentLocationTypeID { get; set; }
        public bool Active { get; set; }
        public short DistributorTypeId { get; set; }
        public string DistributorTypeName { get; set; }
        public string DistributorTypeDesc { get; set; }
    }

    public class DistributorType
    {
        public short DistributorTypeID { get; set; }
        public string DistributorTypeName { get; set; }
        public string DistributorTypeDesc { get; set; }


    }

    public class DistributorAndTypes
    {
        public List<Distributor> Distributors { get; set; }

        public List<DistributorType> DistributorTypes { get; set; }
    }
}
