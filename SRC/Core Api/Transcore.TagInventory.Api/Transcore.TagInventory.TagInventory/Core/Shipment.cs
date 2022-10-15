using System;
using System.Collections.Generic;
using System.Text;

namespace Transcore.TagInventory.Entity.Core
{
    public class Shipment
    {
        public long ShipmentID { get; set; }

        public string ShipmentName { get; set; }

        public string PurchaseOrder { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShipmentDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public List<ReceivedBox> ReceivedBoxes { get; set; }

        public short StatusID { get; set; }

        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int RowNumber { get; set; }
    }
}
