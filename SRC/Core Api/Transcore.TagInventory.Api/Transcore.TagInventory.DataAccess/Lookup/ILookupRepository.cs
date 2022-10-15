using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.DataAccess.Lookup
{
    public interface ILookupRepository
    {
        Dictionary<string, List<KeyValuePair<short, string>>> GetLookup();

        DistributorAndTypes GetDistributors();
    }
}
