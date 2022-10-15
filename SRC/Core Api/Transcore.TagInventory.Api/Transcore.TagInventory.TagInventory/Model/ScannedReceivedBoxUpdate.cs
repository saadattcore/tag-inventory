using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class ScannedReceivedBoxUpdate
    {
       
        public long ReceivedBoxID { get; set; }
     
        public short StatusID { get; set; }

        public int UpdateUserID { get; set; }

        public List<ScannedTagUpdate> ScanTags { get; set; }
    }
}
