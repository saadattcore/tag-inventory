using System;
using System.Collections.Generic;
using System.Text;
using Transcore.TagInventory.Common.Enums;

namespace Transcore.TagInventory.Entity.Core
{
    public class ShipmentFile
    {
        //public ShipmentFile()
        //{
        //    FileContent = new List<string>();
        //}

        public long ShipmentFileID { get; set; }

        public string CaseNumber { get; set; }

        public long CaseID { get; set; }

        public string FileName { get; set; }

        public DateTime OrderProcessedDate { get; set; }

        public short OrderQuantity { get; set; }

        public string PartNumber { get; set; }

        public string SalesOrderNumber { get; set; }

        public long ShipmentID { get; set; }

        public List<ReceivedBox> ReceivedBoxes { get; set; }

        public List<IssuedBox> IssuedBoxes { get; set; }

        public int CreatedUserID { get; set; }

        public int UpdatedUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int FileStatusID { get; set; }

        public string FileStatus { get; set; }

        public string Remarks { get; set; }

        public string Header { get; set; }

        public byte BoxTypeID { get; set; }

        public ExportPackageType PackageType { get; set; }
    }
}
