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

    public class TagRepository : ITagRepository
    {
        private readonly IDataAccess _dataAccess;

        public TagRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public Tag GetTag(long tagID)
        {
            Tag tag = null;

            SqlParameter pTagID = new SqlParameter()
            {
                ParameterName = "@ipv_biTagID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = tagID
            };

            SqlParameter[] spParams = new SqlParameter[1];
            spParams[0] = pTagID;


            DataSet dataSet = _dataAccess.Execute("uspTagGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                tag = dataSet.ToTag();
            }

            return tag;
        }

        public Page<Tag> GetTags(TagSearch searchOptions, int pageSize, int pageNumber)
        {
            Page<Tag> response = null;

            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.ReceivedBoxID ?? Convert.DBNull
            };

            SqlParameter pIssuedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.IssuedBoxID ?? Convert.DBNull
            };

            SqlParameter pTagID = new SqlParameter()
            {
                ParameterName = "@ipv_biTagID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.TagID ?? Convert.DBNull
            };

            SqlParameter pTagNumber = new SqlParameter()
            {
                ParameterName = "@ipv_vcTagNumber",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.TagNumber ?? Convert.DBNull
            };

            SqlParameter pSerialNumber = new SqlParameter()
            {
                ParameterName = "@ipv_biSerialNumber",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.SerialNumber ?? Convert.DBNull
            };

            SqlParameter pIsImported = new SqlParameter()
            {
                ParameterName = "@ipv_bIsImported",
                SqlDbType = System.Data.SqlDbType.Bit,
                Value = searchOptions.IsImported ?? Convert.DBNull
            };

            SqlParameter pVisualCheckStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiVisualCheckStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.VisualCheckStatusID ?? Convert.DBNull
            };

            SqlParameter pVisualCheckStatus = new SqlParameter()
            {
                ParameterName = "@ipv_vcVisualCheckStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.VisualCheckStatus ?? Convert.DBNull
            };

            SqlParameter pRFIDCheckStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiRFIDCheckStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.RFIDCheckStatusID ?? Convert.DBNull
            };

            SqlParameter pRFIDCheckStatus = new SqlParameter()
            {
                ParameterName = "@ipv_vcRFIDCheckStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.RFIDCheckStatus ?? Convert.DBNull
            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiTagStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.StatusID ?? Convert.DBNull
            };

            SqlParameter pTagStatus = new SqlParameter()
            {
                ParameterName = "@ipv_vcTagStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.Status ?? Convert.DBNull
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

            SqlParameter[] spParams = new SqlParameter[14];
            spParams[0] = pReceivedBoxID;
            spParams[1] = pIssuedBoxID;
            spParams[2] = pStatusID;
            spParams[3] = pTagID;
            spParams[4] = pTagNumber;
            spParams[5] = pSerialNumber;
            spParams[6] = pIsImported;
            spParams[7] = pVisualCheckStatusID;
            spParams[8] = pRFIDCheckStatusID;
            spParams[9] = pVisualCheckStatus;
            spParams[10] = pRFIDCheckStatus;
            spParams[11] = pTagStatus;
            spParams[12] = pPageSize;
            spParams[13] = pPageNumber;

            DataSet dataSet = _dataAccess.Execute("uspTagsGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToTags();
            }
            return response;
        }

        public void UpdateTagsStatus(List<Tag> tags)
        {
            DataTable dtTagsStatusUpdate = new DataTable();
            dtTagsStatusUpdate.Columns.Add(new DataColumn("biTagID", typeof(long)));
            dtTagsStatusUpdate.Columns.Add(new DataColumn("tiStatusID", typeof(string)));
            dtTagsStatusUpdate.Columns.Add(new DataColumn("tiVisualCheckStatusID", typeof(DateTime)));
            dtTagsStatusUpdate.Columns.Add(new DataColumn("tiRFIDCheckStatusID", typeof(short)));
            dtTagsStatusUpdate.Columns.Add(new DataColumn("iUpdUserID", typeof(string)));

            foreach (var tag in tags)
            {
                DataRow tagRow = dtTagsStatusUpdate.NewRow();

                tagRow["biTagID"] = tag.TagID;
                tagRow["tiStatusID"] = tag.StatusID;
                tagRow["tiVisualCheckStatusID"] = tag.VisualCheckStatusID;
                //tagRow["bRFIDCheck"] = tag.RFIDCheck;
                tagRow["iUpdUserID"] = tag.UpdatedUserID;

                dtTagsStatusUpdate.Rows.Add(tagRow);
            }

            SqlParameter pTagStatusUpdate = new SqlParameter()
            {
                ParameterName = "@ipv_ttTagStatusUpdate",
                SqlDbType = SqlDbType.Structured,
                Value = dtTagsStatusUpdate
            };

            SqlParameter[] paramList = new SqlParameter[1];
            paramList[0] = pTagStatusUpdate;

            _dataAccess.ExecuteNonQuery("uspReceivedBoxStatusUpd", paramList);


        }


        public List<TagActivityHistory> GetTagHistory(long tagID)
        {
            List<TagActivityHistory> response = null;

            SqlParameter pTagID = new SqlParameter()
            {
                ParameterName = "@ipv_biTagID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = tagID
            };

            SqlParameter[] spParams = new SqlParameter[1];

            spParams[0] = pTagID;

            DataSet dataSet = _dataAccess.Execute("uspTagHistoryGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToTagTimeLine();
            }
            return response;
        }

        public void UpdateTag(Tag tag)
        {
            SqlParameter[] spParams = new SqlParameter[4];

            SqlParameter pTagID = new SqlParameter()
            {
                ParameterName = "@ipv_biTagID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = tag.TagID
            };

            SqlParameter pIPin = new SqlParameter()
            {
                ParameterName = "@ipv_iPin",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = tag.PIN
            };

            SqlParameter pTagStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiTagStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = tag.StatusID
            };

            SqlParameter pUpdUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iUpdUserID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = tag.UpdatedUserID
            };

            spParams[0] = pTagID;
            spParams[1] = pIPin;
            spParams[2] = pTagStatusID;
            spParams[3] = pUpdUserID;

            _dataAccess.ExecuteNonQuery("uspTagUpdate", spParams);


        }

    }
}
