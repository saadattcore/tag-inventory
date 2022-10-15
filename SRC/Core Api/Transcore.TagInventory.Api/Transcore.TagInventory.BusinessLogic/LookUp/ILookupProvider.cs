using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.BusinessLogic.LookUp
{
    public interface ILookupProvider
    {
        List<KeyValuePair<short, string>> GetLookup(string key);

        DistributorAndTypes GetDistributors();
    }
}
