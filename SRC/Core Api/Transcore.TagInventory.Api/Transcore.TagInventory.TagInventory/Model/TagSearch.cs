using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class TagSearch
    {
        public long? TagID { get; set; }

        public string TagNumber { get; set; }

        public long? SerialNumber { get; set; }

        public long? IssuedBoxID { get; set; }

        public long? ReceivedBoxID { get; set; }

        public bool? IsImported { get; set; }

        public short? VisualCheckStatusID { get; set; }

        public string VisualCheckStatus { get; set; }

        public short? RFIDCheckStatusID { get; set; }

        public string RFIDCheckStatus { get; set; }

        public short? StatusID { get; set; }

        public string Status { get; set; }

    }
}
