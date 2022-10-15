using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class IssuedBoxSearch
    {
        public long? IssuedBoxID { get; set; }
        public short? StatusID { get; set; }
        public short? Quantity { get; set; }
        public string Status { get; set; }
    }
}
