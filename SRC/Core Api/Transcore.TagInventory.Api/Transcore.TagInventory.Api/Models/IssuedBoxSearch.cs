using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transcore.TagInventory.Web.Models
{
    public class IssuedBoxSearch
    {
        public long? IssuedBoxID { get; set; }
        public short? StatusID { get; set; }
        public short? Quantity { get; set; }
        public string Status { get; set; }
    }
}