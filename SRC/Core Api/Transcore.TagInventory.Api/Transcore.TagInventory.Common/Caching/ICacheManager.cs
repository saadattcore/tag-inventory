using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;

namespace Transcore.TagInventory.Common.Caching
{
    public interface ICacheManager
    {
        List<KeyValuePair<short,string>> GetValue(string key, Func<string,List<KeyValuePair<short, string>>> getLookUp);
    }
}
