using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.DataAccess.Common;
using Transcore.TagInventory.Entity.Model;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.DataAccess
{
    public class ShipmentRepository : IShipmentRepository
    {
        protected readonly IDataAccess _dataAccess;

        public ShipmentRepository(IDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
        }

        public long Add(Shipment shipment)
        {

            long shipmentID = -1;

            SqlParameter pName = new SqlParameter()
            {
                ParameterName = "@ipv_vcShipmentName",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = shipment.ShipmentName

            };

            SqlParameter pPO = new SqlParameter()
            {
                ParameterName = "@ipv_vcPurchaseOrder",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = shipment.PurchaseOrder

            };

            SqlParameter pOrderDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtOrderDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = shipment.OrderDate

            };

            SqlParameter pShipmentDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtShipmentDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = shipment.ShipmentDate

            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiShipmentStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = shipment.StatusID

            };

            SqlParameter pCreatedUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iCreatedUserID",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = shipment.CreatedBy

            };

            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = pName;
            sqlParams[1] = pPO;
            sqlParams[2] = pOrderDate;
            sqlParams[3] = pShipmentDate;
            sqlParams[4] = pStatusID;
            sqlParams[5] = pCreatedUserID;

            object id = _dataAccess.ExecuteScaler("uspShipmentIns", sqlParams);

            if (id != null)
            {
                shipmentID = Convert.ToInt64(id);
            }


            return shipmentID;

        }

        public void Update(Shipment shipment)
        {

            SqlParameter pShipmentID = new SqlParameter()
            {
                ParameterName = "@ipv_biShipmentID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = shipment.ShipmentID

            };

            SqlParameter pName = new SqlParameter()
            {
                ParameterName = "@ipv_vcShipmentName",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = shipment.ShipmentName

            };

            SqlParameter pPO = new SqlParameter()
            {
                ParameterName = "@ipv_vcPurchaseOrder",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = shipment.PurchaseOrder

            };

            SqlParameter pOrderDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtOrderDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = shipment.OrderDate

            };

            SqlParameter pShipmentDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtShipmentDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = shipment.ShipmentDate

            };

            SqlParameter pDeliveryDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtDeliveryDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = shipment.DeliveryDate

            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiShipmentStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = shipment.StatusID

            };

            SqlParameter pUpdatedUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iUpdUserID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = shipment.UpdatedBy

            };

            SqlParameter[] sqlParams = new SqlParameter[8];
            sqlParams[0] = pShipmentID;
            sqlParams[1] = pName;
            sqlParams[2] = pPO;
            sqlParams[3] = pOrderDate;
            sqlParams[4] = pShipmentDate;
            sqlParams[5] = pDeliveryDate;
            sqlParams[6] = pStatusID;
            sqlParams[7] = pUpdatedUserID;

            _dataAccess.ExecuteNonQuery("uspShipmentUpd", sqlParams);
        }

        public Page<Shipment> GetShipment(ShipmentSearch searchOptions, int pageSize, int pageNumber)
        {
            Page<Shipment> shipments = null;

            SqlParameter pShipmentID = new SqlParameter()
            {
                ParameterName = "@ipv_biShipmentID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.ShipmentID ?? Convert.DBNull
            };

            SqlParameter pPurchaseOrder = new SqlParameter()
            {
                ParameterName = "@ipv_vcPurchaseOrder",                
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = string.IsNullOrEmpty(searchOptions.PurchaseOrder) ? Convert.DBNull : searchOptions.PurchaseOrder.Trim()
            };

            SqlParameter pOrderDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtOrderDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = searchOptions.OrderDate ?? Convert.DBNull
            };

            SqlParameter pShipmentDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtShipmentDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = searchOptions.ShipmentDate ?? Convert.DBNull
            };

            SqlParameter pShipmentStatus = new SqlParameter()
            {
                ParameterName = "@ipv_vcShipmentStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.Status ?? Convert.DBNull
            };

            SqlParameter pShipmentStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiShipmentStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.StatusID ?? Convert.DBNull
            };

            

            SqlParameter pShipmentName = new SqlParameter()
            {
                ParameterName = "@ipv_vcShipmentName",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = string.IsNullOrEmpty(searchOptions.ShipmentName) ? Convert.DBNull : searchOptions.ShipmentName.Trim()
            };

            SqlParameter pPageSize = new SqlParameter()
            {
                ParameterName = "@ipv_iPageSize",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = pageSize
            };

            SqlParameter pPageNumber = new SqlParameter()
            {
                ParameterName = "@ipv_iPageNumber",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = pageNumber
            };

            SqlParameter[] spParams = new SqlParameter[9];
            spParams[0] = pShipmentID;
            spParams[1] = pPurchaseOrder;
            spParams[2] = pOrderDate;
            spParams[3] = pShipmentDate;
            spParams[4] = pShipmentStatus;
            spParams[5] = pShipmentStatusID;
            spParams[6] = pShipmentName;
            spParams[7] = pPageSize;
            spParams[8] = pPageNumber;

            

            DataSet dataSet = _dataAccess.Execute("uspShipmentGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                shipments = dataSet.ToShipment();
            }
            return shipments;
        }


    }
}