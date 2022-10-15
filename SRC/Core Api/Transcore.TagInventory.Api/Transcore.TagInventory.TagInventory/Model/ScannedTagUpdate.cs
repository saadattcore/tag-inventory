using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class ScannedTagUpdate
    {        
        public long TagID { get; set; }

        public string TagNumber { get; set; }

        public long SerialNumber { get; set; }

        public int PIN { get; set; }

        public long ReceivedBoxID { get; set; }

        public bool IsImported { get; set; }

        public short StatusID { get; set; }

        public short VisualCheckStatusID { get; set; }
       
        public short RFIDCheckStatusID { get; set; }

        public int CreatedUserID { get; set; }

        public int UpdatedUserID { get; set; }
    }
}
