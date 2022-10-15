using System;
using System.Collections.Generic;
using System.Text;

namespace Transcore.TagInventory.Entity.Core
{
    public class TagShipmentHeader
    {
        public string CaseNumber { get; set; }

        public string FileName { get; set; }

        public DateTime OrderProcessedDate { get; set; }

        public short OrderQuantity { get; set; }

        public string PartNumber { get; set; }

        public string SalesOrderNumber { get; set; }

        public long ShipmentID { get; set; }

        public List<ReceivedBox> ReceivedBoxes { get; set; }

        public int CreatedUserID { get; set; }

        public int UpdatedUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int FileStatusID { get; set; }

        public string FileStatus { get; set; }

        public string Remarks { get; set; }
    }
}
