using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Common
{
    public class Page<T>
    {   
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int SearchCount { get; set; }
    }
}
