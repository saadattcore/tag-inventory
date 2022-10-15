using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.Entity.Model
{
    public class IssuedBoxActivityHistory
    {
        public DateTime DateGroup { get; set; }

        public List<IssuedBox> IssuedBoxList { get; set; }
    }
}
