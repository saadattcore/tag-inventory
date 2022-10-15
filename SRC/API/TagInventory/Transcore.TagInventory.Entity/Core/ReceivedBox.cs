using System;
using System.Collections.Generic;
using System.Text;

namespace Transcore.TagInventory.Entity.Core
{
    public class ReceivedBox
    {
        public long ReceivedBoxID { get; set; }

        public long StartTag { get; set; }

        public long EndTag { get; set; }

        public long ShipmentID { get; set; }

        public short Quantity { get; set; }

        public long CaseID { get; set; }

        public short StatusID { get; set; }

        public string Status { get; set; }

        public short BoxTypeID { get; set; }

        public string BoxType { get; set; }

        public bool IssuedBoxCreated { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Remarks { get; set; }

        public List<Tag> Tags { get; set; }

        public Shipment Shipment { get; set; }
    }
}
