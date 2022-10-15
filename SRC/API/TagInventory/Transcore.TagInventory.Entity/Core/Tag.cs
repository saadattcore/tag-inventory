using System;
using System.Collections.Generic;
using System.Text;

namespace Transcore.TagInventory.Entity.Core
{
    public class Tag
    {
        public long TagID { get; set; }

        public string TagNumber { get; set; }

        public long SerialNumber { get; set; }

        public long? IssuedBoxID { get; set; }

        public long ReceivedBoxID { get; set; }

        public bool IsImported { get; set; }

        public short VisualCheckStatusID { get; set; }

        public string VisualCheckStatus { get; set; }

        public short RFIDCheckStatusID { get; set; }

        public string RFIDCheckStatus { get; set; }

        public short KitVisualCheckStatusID { get; set; }

        public string KitVisualCheckStatus { get; set; }

        public short KitRFIDCheckStatusID { get; set; }

        public string KitRFIDCheckStatus { get; set; }

        public short StatusID { get; set; }

        public string Status { get; set; }

        public short TagTypeID { get; set; }

        public string TagType { get; set; }

        public string Frame24 { get; set; }

        public string Frame25 { get; set; }

        public string Frame26 { get; set; }

        public string Frame27 { get; set; }

        public string Marking { get; set; }

        public int CreatedUserID { get; set; }

        public int UpdatedUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string Remarks { get; set; }

        public int PIN { get; set; }

        public DateTime UpdatedDatePart { get; set; }

        public ReceivedBox ReceivedBox { get; set; }
    }
}
