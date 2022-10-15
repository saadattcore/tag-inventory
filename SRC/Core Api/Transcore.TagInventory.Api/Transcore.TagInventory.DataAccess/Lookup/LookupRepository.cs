using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.DataAccess.Common;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.DataAccess.Lookup
{
    public class LookupRepository : ILookupRepository
    {

        private readonly IDataAccess _dataAccess;

        public LookupRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;

        }

        public Dictionary<string, List<KeyValuePair<short, string>>> GetLookup()
        {
            DataSet DSLookup = _dataAccess.Execute("uspLookupGet", null);

            return DSLookup.ToLookup();
            
        }

        public DistributorAndTypes GetDistributors()
        {
            DataSet ds = _dataAccess.Execute("uspDistributorGet", null);

            return ds.ToDistributors();
        }
        
    }
}
