using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class ShipmentSearch
    {
        public long?  ShipmentID { get; set; }
        public string ShipmentName { get; set; }
        public string PurchaseOrder { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string Status { get; set; }
        public short? StatusID { get; set; }
    }
}
