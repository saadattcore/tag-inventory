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
    public class ReceivedBoxRepository : IReceivedBoxRepository
    {
        private readonly IDataAccess _dataAccess;

        public ReceivedBoxRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Page<ReceivedBox> GetReceivedBox(ReceivedBoxSearch searchOptions, int pageSize, int pageNumber)
        {
            Page<ReceivedBox> response = null;

            SqlParameter pShipmentID = new SqlParameter()
            {
                ParameterName = "@ipv_biShipmentID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.ShipmentID ?? Convert.DBNull
            };

            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.ReceivedBoxID ?? Convert.DBNull
            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiReceivedBoxStatusID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.StatusID ?? Convert.DBNull
            };

            SqlParameter pStatus = new SqlParameter()
            {
                ParameterName = "@ipv_tiReceivedBoxStatus",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Value = searchOptions.Status ?? Convert.DBNull
            };

            SqlParameter pFrom = new SqlParameter()
            {
                ParameterName = "@ipv_biFrom",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.StartTag ?? Convert.DBNull
            };

            SqlParameter pTo = new SqlParameter()
            {
                ParameterName = "@ipv_biTo",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = searchOptions.EndTag ?? Convert.DBNull
            };

            SqlParameter pQuantity = new SqlParameter()
            {
                ParameterName = "@ipv_siQuantity",
                SqlDbType = System.Data.SqlDbType.BigInt,
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

            SqlParameter[] spParams = new SqlParameter[9];
            spParams[0] = pShipmentID;
            spParams[1] = pReceivedBoxID;
            spParams[2] = pStatusID;
            spParams[3] = pStatus;
            spParams[4] = pFrom;
            spParams[5] = pTo;
            spParams[6] = pQuantity;
            spParams[7] = pPageSize;
            spParams[8] = pPageNumber;

            DataSet dataSet = _dataAccess.Execute("uspReceivedBoxesGet", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToReceivedBox();
            }
            return response;

        }

        public Page<ReceivedBox> Import(List<TagShipmentHeader> fileCollection)
        {
            Page<ReceivedBox> response = null;

            DataTable dtTagShipmentHdr = new DataTable();
            dtTagShipmentHdr.Columns.Add(new DataColumn("vcCaseNumber", typeof(string)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("vcFileName", typeof(string)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("sdtOrderProcessedTime", typeof(DateTime)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("iOrderQuantity", typeof(short)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("vcPartNumber", typeof(string)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("vcSalesOrderNumber", typeof(string)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("biShipmentID", typeof(long)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("iCreatedUserID", typeof(int)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));
            dtTagShipmentHdr.Columns.Add(new DataColumn("vcRemarks", typeof(string)));


            DataTable dtReceivedBox = new DataTable();
            dtReceivedBox.Columns.Add(new DataColumn("biReceivedBoxID", typeof(long)));
            dtReceivedBox.Columns.Add(new DataColumn("iStartTag", typeof(long)));
            dtReceivedBox.Columns.Add(new DataColumn("iEndTag", typeof(long)));
            dtReceivedBox.Columns.Add(new DataColumn("biShipmentID", typeof(int)));
            dtReceivedBox.Columns.Add(new DataColumn("tiQuantity", typeof(short)));
            dtReceivedBox.Columns.Add(new DataColumn("biCaseID", typeof(long)));
            dtReceivedBox.Columns.Add(new DataColumn("tiBoxTypeID", typeof(byte)));
            dtReceivedBox.Columns.Add(new DataColumn("tiStatusID", typeof(short)));
            dtReceivedBox.Columns.Add(new DataColumn("iCreatedUserID", typeof(int)));
            dtReceivedBox.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));
            dtReceivedBox.Columns.Add(new DataColumn("vcRemarks", typeof(string)));

            DataTable dtTag = new DataTable();
            dtTag.Columns.Add(new DataColumn("biTagID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("vcTagNumber", typeof(string)));
            dtTag.Columns.Add(new DataColumn("biSerialNumber", typeof(long)));
            dtTag.Columns.Add(new DataColumn("iPin", typeof(int)));
            dtTag.Columns.Add(new DataColumn("biIssuedBoxID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("biReceivedBoxID", typeof(long)));
            dtTag.Columns.Add(new DataColumn("bIsImported", typeof(bool)));
            dtTag.Columns.Add(new DataColumn("tiVisualCheckStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("tiRFIDCheckStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("tiTagStatusID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("tiTagTypeID", typeof(byte)));
            dtTag.Columns.Add(new DataColumn("vcMarking", typeof(string)));
            dtTag.Columns.Add(new DataColumn("vcFrame24", typeof(string)));
            dtTag.Columns.Add(new DataColumn("vcFrame25", typeof(string)));
            dtTag.Columns.Add(new DataColumn("vcFrame26", typeof(string)));
            dtTag.Columns.Add(new DataColumn("vcFrame27", typeof(string)));
            dtTag.Columns.Add(new DataColumn("vcFileName", typeof(string)));
            dtTag.Columns.Add(new DataColumn("iCreatedUserID", typeof(int)));
            dtTag.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));
            


            foreach (var file in fileCollection)
            {

                DataRow fileRow = dtTagShipmentHdr.NewRow();

                fileRow["vcCaseNumber"] = file.CaseNumber;
                fileRow["vcFileName"] = file.FileName;
                fileRow["sdtOrderProcessedTime"] = file.OrderProcessedDate;
                fileRow["iOrderQuantity"] = file.OrderQuantity;
                fileRow["vcPartNumber"] = file.PartNumber;
                fileRow["vcSalesOrderNumber"] = file.SalesOrderNumber;
                fileRow["biShipmentID"] = file.ShipmentID;
                fileRow["iCreatedUserID"] = file.CreatedUserID;
                fileRow["iUpdUserID"] = file.UpdatedUserID;
                fileRow["vcRemarks"] = file.Remarks;

                dtTagShipmentHdr.Rows.Add(fileRow);


                foreach (var receivedBox in file.ReceivedBoxes)
                {
                    DataRow boxRow = dtReceivedBox.NewRow();

                    boxRow["biReceivedBoxID"] = receivedBox.ReceivedBoxID;
                    boxRow["iStartTag"] = receivedBox.StartTag;
                    boxRow["iEndTag"] = receivedBox.EndTag;
                    boxRow["biShipmentID"] = receivedBox.ShipmentID;
                    boxRow["tiQuantity"] = receivedBox.Quantity;
                    boxRow["biCaseID"] = receivedBox.CaseID;
                    boxRow["tiBoxTypeID"] = receivedBox.BoxTypeID;
                    boxRow["tiStatusID"] = receivedBox.StatusID;
                    boxRow["iCreatedUserID"] = receivedBox.CreatedBy;
                    boxRow["iUpdUserID"] = receivedBox.UpdatedBy;
                    boxRow["vcRemarks"] = receivedBox.Remarks;

                    dtReceivedBox.Rows.Add(boxRow);

                    foreach (var tag in receivedBox.Tags)
                    {
                        DataRow tagRow = dtTag.NewRow();


                        tagRow["biTagID"] = tag.TagID;
                        tagRow["vcTagNumber"] = tag.TagNumber;
                        tagRow["biSerialNumber"] = tag.SerialNumber;
                        tagRow["iPin"] = tag.PIN;
                        tagRow["biIssuedBoxID"] = DBNull.Value;
                        tagRow["biReceivedBoxID"] = tag.ReceivedBoxID;
                        tagRow["bIsImported"] = DBNull.Value;
                        tagRow["tiVisualCheckStatusID"] = DBNull.Value;
                        tagRow["tiRFIDCheckStatusID"] = DBNull.Value;
                        tagRow["tiTagStatusID"] = DBNull.Value;
                        tagRow["tiTagTypeID"] = receivedBox.BoxTypeID;
                        tagRow["vcMarking"] = tag.Marking;
                        tagRow["vcFrame24"] = tag.Frame24;
                        tagRow["vcFrame25"] = tag.Frame25;
                        tagRow["vcFrame26"] = tag.Frame26;
                        tagRow["vcFrame27"] = tag.Frame27;
                        tagRow["vcFileName"] = file.FileName;
                        tagRow["iCreatedUserID"] = tag.CreatedUserID;
                        tagRow["iUpdUserID"] = tag.UpdatedUserID;
                                                                      
                        dtTag.Rows.Add(tagRow);

                    }
                }
            }


            SqlParameter importFileParam = new SqlParameter()
            {
                ParameterName = "@ipv_ttTagShipmentHdr",
                SqlDbType = SqlDbType.Structured,
                Value = dtTagShipmentHdr
            };

            SqlParameter receivedBoxParam = new SqlParameter()
            {
                ParameterName = "@ipv_ttReceivedBox",
                SqlDbType = SqlDbType.Structured,
                Value = dtReceivedBox
            };

            SqlParameter tagParam = new SqlParameter()
            {
                ParameterName = "@ipv_ttTag",
                SqlDbType = SqlDbType.Structured,
                Value = dtTag
            };

            SqlParameter[] spParams = new SqlParameter[3];

            spParams[0] = importFileParam;
            spParams[1] = receivedBoxParam;
            spParams[2] = tagParam;

            //DataSet result = -1;

            DataSet dataSet = _dataAccess.Execute("uspImportReceivedBoxes", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToReceivedBox();
            }

            return response;

        }



        public void UpdateStatus(ReceivedBoxUpdate receivedBoxUpd)
        {
            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Value = receivedBoxUpd.ReceivedBoxID
            };

            SqlParameter pStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiStatusID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = receivedBoxUpd.StatusID
            };

            SqlParameter pUpdUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iUpdUserID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = receivedBoxUpd.UpdUserID
            };

            SqlParameter pboxTypeID = new SqlParameter()
            {
                ParameterName = "@ipv_tiBoxTypeID",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = receivedBoxUpd.BoxTypeID
            };


            SqlParameter[] spParams = new SqlParameter[4];
            spParams[0] = pReceivedBoxID;
            spParams[1] = pStatusID;
            spParams[2] = pUpdUserID;
            spParams[3] = pboxTypeID;

            int rowsEffected = _dataAccess.ExecuteNonQuery("uspReceivedBoxStatusUpd", spParams);
        }

        public ReceivedBox UpdateScannedBox(ScannedReceivedBoxUpdate boxScanTags)
        {
            ReceivedBox response = null;

            DataTable dtTag = new DataTable();

            dtTag.Columns.Add(new DataColumn("biTagID", typeof(long)));

            dtTag.Columns.Add(new DataColumn("vcTagNumber", typeof(string)));

            dtTag.Columns.Add(new DataColumn("biSerialNumber", typeof(long)));

            dtTag.Columns.Add(new DataColumn("iPin", typeof(int)));

            dtTag.Columns.Add(new DataColumn("biIssuedBoxID", typeof(long)));

            dtTag.Columns.Add(new DataColumn("biReceivedBoxID", typeof(long)));

            dtTag.Columns.Add(new DataColumn("bIsImported", typeof(bool)));

            dtTag.Columns.Add(new DataColumn("tiVisualCheckStatusID", typeof(byte)));

            dtTag.Columns.Add(new DataColumn("tiRFIDCheckStatusID", typeof(byte)));

            dtTag.Columns.Add(new DataColumn("tiTagStatusID", typeof(byte)));

            dtTag.Columns.Add(new DataColumn("vcMarking", typeof(string)));

            dtTag.Columns.Add(new DataColumn("vcFrame24", typeof(string)));

            dtTag.Columns.Add(new DataColumn("vcFrame25", typeof(string)));

            dtTag.Columns.Add(new DataColumn("vcFrame26", typeof(string)));

            dtTag.Columns.Add(new DataColumn("vcFrame27", typeof(string)));

            dtTag.Columns.Add(new DataColumn("iCreatedUserID", typeof(int)));

            dtTag.Columns.Add(new DataColumn("iUpdUserID", typeof(int)));

            foreach (var tag in boxScanTags.ScanTags)
            {
                DataRow tagRow = dtTag.NewRow();

                tagRow["biTagID"] = tag.TagID;

                tagRow["vcTagNumber"] = tag.TagNumber;

                tagRow["biSerialNumber"] = tag.SerialNumber;

                tagRow["iPin"] = tag.PIN;

                tagRow["biIssuedBoxID"] = DBNull.Value;

                tagRow["biReceivedBoxID"] = tag.ReceivedBoxID;

                tagRow["bIsImported"] = tag.IsImported;

                tagRow["tiVisualCheckStatusID"] = tag.VisualCheckStatusID;

                tagRow["tiRFIDCheckStatusID"] = tag.RFIDCheckStatusID;

                tagRow["tiTagStatusID"] = tag.StatusID;

                tagRow["vcMarking"] = string.Empty;

                tagRow["vcFrame24"] = string.Empty;

                tagRow["vcFrame25"] = string.Empty;

                tagRow["vcFrame26"] = string.Empty;

                tagRow["vcFrame27"] = string.Empty;

                tagRow["iCreatedUserID"] = tag.CreatedUserID;

                tagRow["iUpdUserID"] = tag.UpdatedUserID;

                dtTag.Rows.Add(tagRow);

            }

            SqlParameter pTagTable = new SqlParameter()
            {
                ParameterName = "@ipv_ttTag",
                SqlDbType = SqlDbType.Structured,
                Value = dtTag
            };

            SqlParameter pReceivedBoxID = new SqlParameter()
            {
                ParameterName = "@ipv_biReceivedBoxID",
                SqlDbType = SqlDbType.BigInt,
                Value = boxScanTags.ReceivedBoxID
            };

            SqlParameter pReceivedBoxStatusID = new SqlParameter()
            {
                ParameterName = "@ipv_tiReceivedBoxStatusID",
                SqlDbType = SqlDbType.TinyInt,
                Value = boxScanTags.StatusID
            };

            SqlParameter pUpdUserID = new SqlParameter()
            {
                ParameterName = "@ipv_iUpdUserID",
                SqlDbType = SqlDbType.Int,
                Value = boxScanTags.UpdateUserID
            };

            SqlParameter[] spParams = new SqlParameter[4];

            spParams[0] = pTagTable;

            spParams[1] = pReceivedBoxID;

            spParams[2] = pReceivedBoxStatusID;

            spParams[3] = pUpdUserID;

            DataSet dataSet = _dataAccess.Execute("uspReceivedBoxScanTagsUpd", spParams);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                response = dataSet.ToReceivedBoxScanTags();
            }

            return response;

        }
    }
}
