using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{

    public class SerialList
    {
        public List<SerialListHDR> SerialListHDR { get; set; }
    }

    public class SerialListHDR
    {
        public long? IssuedBoxID { get; set; }
        public List<SerialNumberRange> SerialRange { get; set; }
    }

    public class SerialNumberRange
    {
        public long From { get; set; }
        public long To { get; set; }
        public int Qty { get; set; }
    }
}
