using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class ReceivedBoxSearch
    {
        public long? ReceivedBoxID { get; set; }

        public long? StartTag { get; set; }

        public long? EndTag { get; set; }

        public long? ShipmentID { get; set; }

        public short? Quantity { get; set; }

        public short? StatusID { get; set; }
        public string Status { get; set; }
    }
}
