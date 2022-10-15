using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.DataAccess.Common;

namespace Transcore.TagInventory.DataAccess.ExportPackage
{
    public class ExportPackageRepository : IExportPackageRepository
    {
        protected readonly IDataAccess _dataAccess;

        public ExportPackageRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<ShipmentFile> GetExportPackage(long shipmentId, int batchSize)
        {
            SqlParameter pShipmentID = new SqlParameter()
            {
                ParameterName = "@ipv_biShipmentID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = shipmentId
            };

            SqlParameter pBatchSize = new SqlParameter()
            {
                ParameterName = "@ipv_iBatchSize",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = batchSize
            };

            SqlParameter[] spParams = new SqlParameter[2];
            spParams[0] = pShipmentID;
            spParams[1] = pBatchSize;

            DataSet ds = _dataAccess.Execute("uspExportPackage", spParams);

            return ds.ToExportPackage();
        }
    }
}
