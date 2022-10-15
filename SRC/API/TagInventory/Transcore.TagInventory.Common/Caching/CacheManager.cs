using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;

namespace Transcore.TagInventory.Common.Caching
{
    public class CacheManager : ICacheManager
    {
        private MemoryCache _cache;

        public CacheManager()
        {
            _cache = MemoryCache.Default;
        }
        public List<KeyValuePair<short, string>> GetValue(string key, Func<string, List<KeyValuePair<short, string>>> getLookUp)
        {
            List<KeyValuePair<short, string>> lookUp = null;

            if (!_cache.Contains(key))
            {
                lookUp = getLookUp(key);

                _cache.Add(key, lookUp, null);
            }

            lookUp = (List<KeyValuePair<short, string>>)_cache[key];

            return lookUp;
        }
    }
}
