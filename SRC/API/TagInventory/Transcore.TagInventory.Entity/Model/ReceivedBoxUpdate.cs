using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Model
{
    public class ReceivedBoxUpdate
    {

        public long ReceivedBoxID { get; set; }

        public short StatusID { get; set; }

        public short BoxTypeID { get; set; }

        public int UpdUserID { get; set; }

    }
}
