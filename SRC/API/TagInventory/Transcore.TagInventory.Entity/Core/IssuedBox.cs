using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Core
{
    public class IssuedBox
    {
        public long IssuedBoxID { get; set; }

        public short Quantity { get; set; }

        public long ReceivedBoxID { get; set; }

        public long ShipmentID { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public short? DistributorID  { get; set; }

        public string Distributor { get; set; }

        public DateTime? IssuedDate { get; set; }

        public short StatusID { get; set; }

        public string Status { get; set; }

        public bool InitialAssigned { get; set; }

        public int CreatedUserID { get; set; }

        public int? UpdateUserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime UpdatedDatePart { get; set; }

        public string Remarks { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
