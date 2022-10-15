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
    public class IssuedBoxRepository : IIssuedBoxRepository
    {
        private IDataAccess _dataAccess;

        public IssuedBoxRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public long Add(IssuedBox issuedBox)
        {
            long issuedBoxID = -1;

            SqlParameter pQuantity = new SqlParameter()
            {
                ParameterName = "@ipv_siQuantity",
                SqlDbType = System.Data.SqlDbType.SmallInt,
                Value = issuedBox.Quantity

            };

            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBox.ReceivedBoxID

            };

            SqlParameter pIssuedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBox.IssuedBoxID

            };


            SqlParameter pIssuedStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiIssuedBoxStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = issuedBox.StatusID

            };

            SqlParameter pCreatedUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iCreatedUserID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = issuedBox.CreatedUserID

            };



            DataTable dtTag = new DataTable();
            dtTag.Columns.Add(new DataColumn("biTagID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("tiTagStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("biIssuedBoxID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("tiKitVisualCheckStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("tiKitRFIDCheckStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));

            foreach (var tag in issuedBox.Tags)
            {
                DataRow tagRow = dtTag.NewRow();

                tagRow["biTagID"] = tag.TagID;
                tagRow["tiTagStatusID"] = DBNull.Value;
                tagRow["biIssuedBoxID"] = DBNull.Value;
                tagRow["tiKitVisualCheckStatusID"] = DBNull.Value;
                tagRow["tiKitRFIDCheckStatusID"] = DBNull.Value;            
                tagRow["iUpdUserID"] = tag.UpdatedUserID;
                dtTag.Rows.Add(tagRow);

            }

            SqlParameter pTags = new SqlParameter()
            {
                ParameterName = "@ipv_ttTagsUpdate",
                SqlDbType = System.Data.SqlDbType.Structured,
                Value = dtTag

            };



            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = pQuantity;
            sqlParams[1] = pReceivedBoxID;
            sqlParams[2] = pIssuedStatusID;
            sqlParams[3] = pCreatedUserID;
            sqlParams[4] = pTags;
            sqlParams[5] = pIssuedBoxID;

           

            object id = _dataAccess.ExecuteScaler("uspIssuedBoxIns", sqlParams);

            if (id != null)
            {
                issuedBoxID = Convert.ToInt64(id);
            }


            return issuedBoxID;
        }


        public long UpdateBoxAndTags(IssuedBox issuedBox, bool updateIssuedBoxKits)
        {
            long issuedBoxID = -1;

            SqlParameter pQuantity = new SqlParameter()
            {
                ParameterName = "@ipv_siQuantity",
                SqlDbType = System.Data.SqlDbType.SmallInt,
                Value = issuedBox.Quantity

            };

            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBox.ReceivedBoxID

            };

            SqlParameter pIssuedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBox.IssuedBoxID

            };



            SqlParameter pIssuedStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiIssuedBoxStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = issuedBox.StatusID

            };

            SqlParameter pCreatedUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iCreatedUserID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = issuedBox.CreatedUserID

            };



            DataTable dtTag = new DataTable();
            dtTag.Columns.Add(new DataColumn("biTagID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("tiTagStatusID", typeof(short)));
            dtTag.Columns.Add(new DataColumn("biIssuedBoxID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("tiKitVisualCheckStatusID", typeof(short)));
            dtTag.Columns.Add(new DataColumn("tiKitRFIDCheckStatusID", typeof(short)));
            dtTag.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));

            foreach (var tag in issuedBox.Tags)
            {
                DataRow tagRow = dtTag.NewRow();

                tagRow["biTagID"] = tag.TagID;
                tagRow["tiTagStatusID"] = tag.StatusID;

                if (tag.IssuedBoxID != null)
                {
                    tagRow["biIssuedBoxID"] = tag.IssuedBoxID;
                }
                else
                {
                    tagRow["biIssuedBoxID"] = DBNull.Value;
                }

                tagRow["tiKitVisualCheckStatusID"] = tag.KitVisualCheckStatusID;
                tagRow["tiKitRFIDCheckStatusID"] = tag.KitRFIDCheckStatusID;
                tagRow["iUpdUserID"] = tag.UpdatedUserID;
                dtTag.Rows.Add(tagRow);

            }

            SqlParameter pTags = new SqlParameter()
            {
                ParameterName = "@ipv_ttTagsUpdate",
                SqlDbType = System.Data.SqlDbType.Structured,
                Value = dtTag

            };



            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = pQuantity;
            sqlParams[1] = pReceivedBoxID;
            sqlParams[2] = pIssuedBoxID;
            sqlParams[3] = pIssuedStatusID;
            sqlParams[4] = pCreatedUserID;
            sqlParams[5] = pTags;

            object id = updateIssuedBoxKits ? _dataAccess.ExecuteScaler("uspIssuedBoxKitsUpd", sqlParams) : _dataAccess.ExecuteScaler("uspIssuedBoxTagsUpd", sqlParams);

            if (id != null)
            {
                issuedBoxID = Convert.ToInt64(id);
            }


            return issuedBoxID;
        }


        public Page<IssuedBox> GetIssuedBox(IssuedBoxSearch searchOptions, int pageSize, int pageNumber)
        {
            Page<IssuedBox> issuedBoxes = null;

            SqlParameter pIssueBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.IssuedBoxID ?? Convert.DBNull
            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiIssuedBoxStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.StatusID ?? Convert.DBNull
            };

            SqlParameter pStatus = new SqlParameter()
            {
                ParameterName = "@ipv_vcIssuedBoxStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = string.IsNullOrEmpty(searchOptions.Status) ? Convert.DBNull : searchOptions.Status
            };


            SqlParameter pQuantity = new SqlParameter()
            {
                ParameterName = "@ipv_siQuantity",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = searchOptions.Quantity ?? Convert.DBNull
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

            SqlParameter[] spParams = new SqlParameter[6];
            spParams[0] = pIssueBoxID;
            spParams[1] = pStatusID;
            spParams[2] = pStatus;
            spParams[3] = pQuantity;
            spParams[4] = pPageSize;
            spParams[5] = pPageNumber;

            DataSet dataSet = _dataAccess.Execute("uspIssuedBoxGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                issuedBoxes = dataSet.ToIssuedBoxes();
            }
            return issuedBoxes;
        }

        public void Update(IssuedBox issuedBox)
        {
            SqlParameter pIssuedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBox.IssuedBoxID

            };

            SqlParameter pQuantity = new SqlParameter()
            {
                ParameterName = "@ipv_siQuantity",
                SqlDbType = System.Data.SqlDbType.SmallInt,
                Value = issuedBox.Quantity

            };

            SqlParameter pSendDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtSendDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = issuedBox.SendDate

            };

            SqlParameter pReceivedDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtReceivedDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = issuedBox.ReceivedDate

            };

            SqlParameter pDistributor = new SqlParameter()
            {
                ParameterName = "@ipv_siDistributor",
                SqlDbType = System.Data.SqlDbType.SmallInt,
                Value = issuedBox.DistributorID

            };

            SqlParameter pIssuedDate = new SqlParameter()
            {
                ParameterName = "@ipv_dtIssuedDate",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = issuedBox.IssuedDate

            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiIssuedStatusID",
                SqlDbType = System.Data.SqlDbType.TinyInt,
                Value = issuedBox.StatusID

            };

            

            SqlParameter pUpdateUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iUpdUserID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = issuedBox.UpdateUserID

            };

            SqlParameter[] sqlParams = new SqlParameter[8];
            sqlParams[0] = pIssuedBoxID;
            sqlParams[1] = pQuantity;
            sqlParams[2] = pSendDate;
            sqlParams[3] = pReceivedDate;
            sqlParams[4] = pDistributor;
            sqlParams[5] = pIssuedDate;
            sqlParams[6] = pStatusID;
            sqlParams[7] = pUpdateUserID;


            _dataAccess.ExecuteNonQuery("uspIssuedBoxUpd", sqlParams);
        }

        public DataSet GetReportData(string issuedBoxIDList)
        {
            SqlParameter pIssueBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_vcIssuedBoxIdList",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = issuedBoxIDList
            };

            SqlParameter[] spParams = new SqlParameter[1];
            spParams[0] = pIssueBoxID;

            DataSet ds = _dataAccess.Execute("uspIssuedBoxLebelGet", spParams);

            return ds;

        }

        public SerialList GetSerialListData(string issuedBoxIDList, long? shipmentID)
        {
            SqlParameter pIssueBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_vcIssuedBoxIdList",
                SqlDbType = System.Data.SqlDbType.VarChar
            };

            if (string.IsNullOrEmpty(issuedBoxIDList))
            {
                pIssueBoxID.Value = DBNull.Value;
            }
            else
            {
                pIssueBoxID.Value = issuedBoxIDList;
            }

            SqlParameter pShipment = new SqlParameter()
            {
                ParameterName = "@ipv_biShipmentID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                
            };

            if (shipmentID == null)
            {
                pShipment.Value = null;

            }
            else
            {
                pShipment.Value = shipmentID;
            }



            SqlParameter[] spParams = new SqlParameter[2];
            spParams[0] = pIssueBoxID;
            spParams[1] = pShipment;

            DataSet ds = _dataAccess.Execute("uspIssuedBoxLebelGet", spParams);

            return ds.ToSerialList();
        }

        public void UpdateBoxesStatus(List<IssuedBox> boxList)
        {

            DataTable dtBoxList = new DataTable();

            dtBoxList.Columns.Add(new DataColumn("biIssuedBoxID", typeof(long)));
            dtBoxList.Columns.Add(new DataColumn("tiIssuedBoxStatusID", typeof(short)));
            dtBoxList.Columns.Add(new DataColumn("siDistributor", typeof(short)));
            dtBoxList.Columns.Add(new DataColumn("dtIssuedDate", typeof(DateTime)));
            dtBoxList.Columns.Add(new DataColumn("dtSendDate", typeof(DateTime)));            
            dtBoxList.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));
            dtBoxList.Columns.Add(new DataColumn("vcRemarks", typeof(string)));


            foreach (var box in boxList)
            {
                DataRow row = dtBoxList.NewRow();

                row["biIssuedBoxID"] = box.IssuedBoxID;
                row["tiIssuedBoxStatusID"] = box.StatusID;

                if (box.DistributorID != null)
                    row["siDistributor"] = box.DistributorID;
                if(box.IssuedDate != null)
                    row["dtIssuedDate"] = box.IssuedDate;
                if(box.SendDate != null)
                    row["dtSendDate"] = box.SendDate;
                
                row["iUpdUserID"] = box.UpdateUserID;             
                row["vcRemarks"] = box.Remarks;

                dtBoxList.Rows.Add(row);
            }

            SqlParameter pBoxList = new SqlParameter()
            {
                ParameterName = "@ipv_ttIssuedBox",
                SqlDbType = SqlDbType.Structured,
                Value = dtBoxList
            };

            SqlParameter[] paramList = new SqlParameter[1];
            paramList[0] = pBoxList;

            _dataAccess.ExecuteNonQuery("uspIssuedBoxStatusUpd", paramList);

        }



        public List<IssuedBoxActivityHistory> GetIssuedBoxHistory(long issuedBoxID)
        {
            List<IssuedBoxActivityHistory> response = null;

            SqlParameter pTagID = new SqlParameter()
            {
                ParameterName = "@ipv_biIssuedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = issuedBoxID
            };

            SqlParameter[] spParams = new SqlParameter[1];

            spParams[0] = pTagID;

            DataSet dataSet = _dataAccess.Execute("uspIssuedBoxHistoryGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToIssuedBoxTimeLine();
            }
            return response;
        }

    }
}
