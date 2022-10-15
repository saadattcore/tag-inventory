using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.DataAccess.Lookup;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.BusinessLogic.LookUp
{
    public class LookupProvider : ILookupProvider
    {
        private readonly ILookupRepository _repository;
        private readonly Dictionary<string, List<KeyValuePair<short, string>>> _lookUps;

        public LookupProvider(ILookupRepository repository)
        {
            _repository = repository;
            if (_lookUps == null)
                _lookUps = repository.GetLookup();
        }

        public DistributorAndTypes GetDistributors()
        {
            return _repository.GetDistributors();
        }

        public List<KeyValuePair<short, string>> GetLookup(string key)
        {
            return _lookUps[key];
        }

    }
}
