using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Model;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.DataAccess.Common
{
    public static class DataSetExtensions
    {
        public static Page<Shipment> ToShipment(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            Page<Shipment> response = new Page<Shipment>()
            {
                Data = new List<Shipment>(),
                SearchCount = 0,
                TotalCount = 0
            };


            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Shipment shipment = new Shipment();

                shipment.ShipmentID = Convert.ToInt64(row["biShipmentID"]);
                shipment.ShipmentName = row["vcShipmentName"].ToString();
                shipment.PurchaseOrder = row["vcPurchaseOrder"].ToString();
                shipment.OrderDate = Convert.ToDateTime(row["dtOrderDate"]);

                if (DBNull.Value != row["dtDeliveryDate"])
                {
                    shipment.DeliveryDate = Convert.ToDateTime(row["dtDeliveryDate"]);
                }

                shipment.ShipmentDate = Convert.ToDateTime(row["dtShipmentDate"]);
                shipment.StatusID = Convert.ToInt16(row["tiShipmentStatusID"]);
                shipment.Status = row["vcShipmentStatus"].ToString();
                shipment.CreatedDate = Convert.ToDateTime(row["dtCreatedTime"]);
                shipment.CreatedBy = Convert.ToInt32(row["iCreatedUserID"]);
                shipment.UpdatedBy = Convert.ToInt32(row["iUpdUserID"]);

                if (DBNull.Value != row["dtUpdTime"])
                {
                    shipment.UpdatedDate = Convert.ToDateTime(row["dtUpdTime"]);
                }

                shipment.RowNumber = Convert.ToInt32(row["RowNumber"]);

                response.Data.Add(shipment);
            }

            var searchCount = dataSet.Tables[1].Rows[0]["SearchCount"];
            var totalCount = dataSet.Tables[2].Rows[0]["TotalCount"];


            if (searchCount != null)
            {
                response.SearchCount = Convert.ToInt32(searchCount);
            }

            if (totalCount != null)
            {
                response.TotalCount = Convert.ToInt32(totalCount);
            }

            return response;
        }


        public static ReceivedBox ToReceivedBoxScanTags(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            var receivedBoxList = GetReceivedBox(dataSet.Tables[0]);

            ReceivedBox rb = null;

            if (receivedBoxList.Count > 0)
            {
                rb = receivedBoxList[0];

                var tags = GetTags(dataSet.Tables[1]);

                rb.Tags = new List<Tag>();

                rb.Tags.AddRange(tags);
            }

            return rb;

        }


        public static Page<ReceivedBox> ToReceivedBox(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            Page<ReceivedBox> response = new Page<ReceivedBox>()
            {
                Data = new List<ReceivedBox>(),
                SearchCount = 0,
                TotalCount = 0
            };

            var rbList = GetReceivedBox(dataSet.Tables[0]);

            response.Data.AddRange(rbList);

            if (dataSet.Tables.Count > 1)
            {

                var searchCount = 0;
                var totalCount = 0;

                if (dataSet.Tables[1].Columns.Contains("SearchCount") && DBNull.Value != dataSet.Tables[1].Rows[0]["SearchCount"])
                    searchCount = Convert.ToInt32(dataSet.Tables[1].Rows[0]["SearchCount"]);

                if (dataSet.Tables[2].Columns.Contains("TotalCount") && DBNull.Value != dataSet.Tables[2].Rows[0]["TotalCount"])
                    totalCount = Convert.ToInt32(dataSet.Tables[2].Rows[0]["TotalCount"]);

                response.SearchCount = Convert.ToInt32(searchCount);

                response.TotalCount = Convert.ToInt32(totalCount);

                var tags = GetTags(dataSet.Tables[3].Copy());

                var join = response.Data.Join(tags, rb => rb.ReceivedBoxID, t => t.ReceivedBoxID, (rb, t) => new { RB = rb, T = t }).ToList();

                foreach (ReceivedBox box in response.Data)
                {
                    var boxTags = join.FindAll(r => r.T.ReceivedBoxID == box.ReceivedBoxID).Select(r => r.T).ToList();

                    if (boxTags != null && boxTags.Count > 0)
                    {
                        box.Tags = new List<Tag>();
                        box.Tags.AddRange(boxTags);
                    }

                }
            }



            return response;
        }

        private static List<ReceivedBox> GetReceivedBox(DataTable rbDT)
        {
            if (rbDT == null)
                throw new ArgumentNullException(nameof(rbDT));

            List<ReceivedBox> rbList = new List<ReceivedBox>();

            foreach (DataRow row in rbDT.Rows)
            {
                ReceivedBox rb = new ReceivedBox();

                if (rbDT.Columns.Contains("biReceivedBoxID") && DBNull.Value != row["biReceivedBoxID"])
                {
                    rb.ReceivedBoxID = Convert.ToInt64(row["biReceivedBoxID"]);
                }

                if (rbDT.Columns.Contains("biStartTag") && DBNull.Value != row["biStartTag"])
                {
                    rb.StartTag = Convert.ToInt64(row["biStartTag"]);
                }

                if (rbDT.Columns.Contains("biEndTag") && DBNull.Value != row["biEndTag"])
                {
                    rb.EndTag = Convert.ToInt64(row["biEndTag"]);
                }


                if (rbDT.Columns.Contains("biTagShipmentHdr") && DBNull.Value != row["biTagShipmentHdr"])
                {
                    rb.ShipmentID = Convert.ToInt64(row["biTagShipmentHdr"]);
                }

                if (rbDT.Columns.Contains("siQuantity") && DBNull.Value != row["siQuantity"])
                {
                    rb.Quantity = Convert.ToInt16(row["siQuantity"]);
                }

                if (rbDT.Columns.Contains("tiReceivedBoxStatusID") && DBNull.Value != row["tiReceivedBoxStatusID"])
                {
                    rb.StatusID = Convert.ToInt16(row["tiReceivedBoxStatusID"]);
                }

                if (rbDT.Columns.Contains("tiBoxTypeID") && DBNull.Value != row["tiBoxTypeID"])
                {
                    rb.BoxTypeID = Convert.ToInt16(row["tiBoxTypeID"]);
                }


                if (rbDT.Columns.Contains("vcBoxStatus") && DBNull.Value != row["vcBoxStatus"])
                {
                    rb.Status = row["vcBoxStatus"].ToString();
                }

                if (rbDT.Columns.Contains("iCreatedUserID") && DBNull.Value != row["iCreatedUserID"])
                {
                    rb.CreatedBy = Convert.ToInt32(row["iCreatedUserID"]);
                }

                if (rbDT.Columns.Contains("iUpdUserID") && DBNull.Value != row["iUpdUserID"])
                {
                    rb.UpdatedBy = Convert.ToInt32(row["iUpdUserID"]);
                }

                if (rbDT.Columns.Contains("dtCreatedTime") && DBNull.Value != row["dtCreatedTime"])
                {
                    rb.CreatedDate = Convert.ToDateTime(row["dtCreatedTime"]);
                }

                if (rbDT.Columns.Contains("dtUpdTime") && DBNull.Value != row["dtUpdTime"])
                {
                    rb.UpdatedDate = Convert.ToDateTime(row["dtUpdTime"]);
                }

                if (rbDT.Columns.Contains("bIssuedBoxCreated") && DBNull.Value != row["bIssuedBoxCreated"])
                {
                    rb.IssuedBoxCreated = Convert.ToBoolean(row["bIssuedBoxCreated"]);
                }

                rb.Shipment = new Shipment();

                if (rbDT.Columns.Contains("vcShipmentName") && DBNull.Value != row["vcShipmentName"])
                {
                    rb.Shipment.ShipmentName = row["vcShipmentName"].ToString();
                }

                if (rbDT.Columns.Contains("vcPurchaseOrder") && DBNull.Value != row["vcPurchaseOrder"])
                {
                    rb.Shipment.PurchaseOrder = row["vcPurchaseOrder"].ToString();
                }

                if (rbDT.Columns.Contains("biTagShipmentHdrID") && DBNull.Value != row["biTagShipmentHdrID"])
                {
                    rb.ShipmentFileID = Convert.ToInt64(row["biTagShipmentHdrID"]);
                }

                if (rbDT.Columns.Contains("biIssuedBoxID") && DBNull.Value != row["biIssuedBoxID"])
                {
                    rb.IssuedBoxID = Convert.ToInt64(row["biIssuedBoxID"]);
                }

                


                rbList.Add(rb);
            }
            return rbList;
        }

        public static List<TagActivityHistory> ToTagTimeLine(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            //TagActivityHistory timeLine = new TagActivityHistory();

            List<DateTime> dateGroup = new List<DateTime>();

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Columns.Contains("dtUpdDate"))
            {
                //timeLine.DateGroup = new List<DateTime>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    if (row["dtUpdDate"] != DBNull.Value)
                    {
                        dateGroup.Add(Convert.ToDateTime(row["dtUpdDate"]));
                    }
                }
            }

            var tags = GetTags(dataSet.Tables[1]);

            List<TagActivityHistory> tagsActivityHistory = new List<TagActivityHistory>();

            if (tags != null && tags.Count > 0)
            {


                foreach (DateTime grp in dateGroup)
                {
                    var tagsGroup = tags.FindAll(t => t.UpdatedDatePart == grp);

                    tagsActivityHistory.Add(new TagActivityHistory() { DateGroup = grp, Tags = tagsGroup });
                }
            }


            return tagsActivityHistory;

        }


        public static List<IssuedBoxActivityHistory> ToIssuedBoxTimeLine(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            //TagActivityHistory timeLine = new TagActivityHistory();

            List<DateTime> dateGroup = new List<DateTime>();

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Columns.Contains("dtUpdDate"))
            {
                //timeLine.DateGroup = new List<DateTime>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    if (row["dtUpdDate"] != DBNull.Value)
                    {
                        dateGroup.Add(Convert.ToDateTime(row["dtUpdDate"]));
                    }
                }
            }

            var issuedBoxList = GetIssuedBoxList(dataSet.Tables[1]);

            List<IssuedBoxActivityHistory> issuedBoxActivityHistory = new List<IssuedBoxActivityHistory>();

            if (issuedBoxList != null && issuedBoxList.Count > 0)
            {


                foreach (DateTime grp in dateGroup)
                {
                    var issuedGroup = issuedBoxList.FindAll(t => t.UpdatedDatePart == grp);

                    issuedBoxActivityHistory.Add(new IssuedBoxActivityHistory() { DateGroup = grp, IssuedBoxList = issuedGroup });
                }
            }


            return issuedBoxActivityHistory;

        }

        public static Page<Tag> ToTags(this DataSet dataSet)
        {

            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            Page<Tag> response = new Page<Tag>()
            {
                Data = new List<Tag>(),
                SearchCount = 0,
                TotalCount = 0
            };

            var tags = GetTags(dataSet.Tables[0]);

            var searchCount = 0;
            var totalCount = 0;



            if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Columns.Contains("SearchCount") && DBNull.Value != dataSet.Tables[1].Rows[0]["SearchCount"])
                searchCount = Convert.ToInt32(dataSet.Tables[1].Rows[0]["SearchCount"]);

            if (dataSet.Tables.Count > 2 && dataSet.Tables[2].Columns.Contains("TotalCount") && DBNull.Value != dataSet.Tables[2].Rows[0]["TotalCount"])
                totalCount = Convert.ToInt32(dataSet.Tables[2].Rows[0]["TotalCount"]);

            response.SearchCount = Convert.ToInt32(searchCount);

            response.TotalCount = Convert.ToInt32(totalCount);

            response.Data.AddRange(tags);

            return response;
        }

        public static List<Tag> GetTags(DataTable dt)
        {
            List<Tag> tags = new List<Tag>();

            foreach (DataRow row in dt.Rows)
            {
                Tag tag = new Tag();

                tag.TagID = Convert.ToInt64(row["biTagID"]);

                tag.TagNumber = row["vcTagNumber"].ToString();

                tag.SerialNumber = Convert.ToInt64(row["biSerialNumber"]);

                if (dt.Columns.Contains("biIssuedBoxID") && DBNull.Value != row["biIssuedBoxID"])
                {
                    tag.IssuedBoxID = Convert.ToInt16(row["biIssuedBoxID"]);
                }

                if (dt.Columns.Contains("biReceivedBoxID") && DBNull.Value != row["biReceivedBoxID"])
                    tag.ReceivedBoxID = Convert.ToInt64(row["biReceivedBoxID"]);

                if (dt.Columns.Contains("iPin") && DBNull.Value != row["iPin"])
                    tag.PIN = Convert.ToInt16(row["iPin"]);

                if (dt.Columns.Contains("bIsImported") && DBNull.Value != row["bIsImported"])
                    tag.IsImported = Convert.ToBoolean(row["bIsImported"]);

                if (dt.Columns.Contains("tiVisualCheckStatusID") && DBNull.Value != row["tiVisualCheckStatusID"])
                    tag.VisualCheckStatusID = Convert.ToInt16(row["tiVisualCheckStatusID"]);

                if (dt.Columns.Contains("vcVisualCheckStatus") && DBNull.Value != row["vcVisualCheckStatus"])
                    tag.VisualCheckStatus = row["vcVisualCheckStatus"].ToString();

                if (dt.Columns.Contains("tiRFIDCheckStatusID") && DBNull.Value != row["tiRFIDCheckStatusID"])
                    tag.RFIDCheckStatusID = Convert.ToInt16(row["tiRFIDCheckStatusID"]);

                if (dt.Columns.Contains("vcRFIDCheckStatus") && DBNull.Value != row["vcRFIDCheckStatus"])
                    tag.RFIDCheckStatus = row["vcRFIDCheckStatus"].ToString();

                if (dt.Columns.Contains("tiKitVisualCheckStatusID") && DBNull.Value != row["tiKitVisualCheckStatusID"])
                    tag.VisualCheckStatusID = Convert.ToInt16(row["tiKitVisualCheckStatusID"]);

                if (dt.Columns.Contains("vcKitVisualCheckStatus") && DBNull.Value != row["vcKitVisualCheckStatus"])
                    tag.VisualCheckStatus = row["vcKitVisualCheckStatus"].ToString();

                if (dt.Columns.Contains("tiKitRFIDCheckStatusID") && DBNull.Value != row["tiKitRFIDCheckStatusID"])
                    tag.RFIDCheckStatusID = Convert.ToInt16(row["tiKitRFIDCheckStatusID"]);

                if (dt.Columns.Contains("vcKitRFIDCheckStatus") && DBNull.Value != row["vcKitRFIDCheckStatus"])
                    tag.RFIDCheckStatus = row["vcKitRFIDCheckStatus"].ToString();

                if (dt.Columns.Contains("tiTagStatusID") && DBNull.Value != row["tiTagStatusID"])
                    tag.StatusID = Convert.ToInt16(row["tiTagStatusID"]);

                if (dt.Columns.Contains("vcTagStatus") && DBNull.Value != row["vcTagStatus"])
                    tag.Status = row["vcTagStatus"].ToString();

                if (dt.Columns.Contains("tiTagTypeID") && DBNull.Value != row["tiTagTypeID"])
                    tag.TagTypeID = Convert.ToInt16(row["tiTagTypeID"]);

                if (dt.Columns.Contains("vcTagType") && DBNull.Value != row["vcTagType"])
                    tag.TagType = row["vcTagType"].ToString();

                if (dt.Columns.Contains("dtCreatedTime") && DBNull.Value != row["dtCreatedTime"])
                    tag.CreatedDate = Convert.ToDateTime(row["dtCreatedTime"]);

                if (dt.Columns.Contains("iCreatedUserID") && DBNull.Value != row["iCreatedUserID"])
                    tag.CreatedUserID = Convert.ToInt32(row["iCreatedUserID"]);

                if (dt.Columns.Contains("iUpdUserID") && DBNull.Value != row["iUpdUserID"])
                    tag.UpdatedUserID = Convert.ToInt32(row["iUpdUserID"]);

                if (dt.Columns.Contains("dtUpdTime") && DBNull.Value != row["dtUpdTime"])
                {
                    tag.UpdatedDate = Convert.ToDateTime(row["dtUpdTime"]);
                }

                if (dt.Columns.Contains("dtUpdDate") && DBNull.Value != row["dtUpdDate"])
                {
                    tag.UpdatedDatePart = Convert.ToDateTime(row["dtUpdDate"]);
                }

                if (dt.Columns.Contains("vcFileName") && DBNull.Value != row["vcFileName"])
                {
                    tag.ShipmentFileName = row["vcFileName"].ToString();
                }

                if (dt.Columns.Contains("bCostTag") && DBNull.Value != row["bCostTag"])
                {
                    tag.CostTag = Convert.ToBoolean(row["bCostTag"]);
                }


                //tag.Remarks = row["vcRemarks"].ToString();    

                tags.Add(tag);
            }

            return tags;
        }

        public static Tag ToTag(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            Tag tag = new Tag();

            var dt = dataSet.Tables[0];
            DataRow row = dataSet.Tables[0].Rows[0];

            if (dt.Columns.Contains("biTagID") && DBNull.Value != row["biTagID"])
            {
                tag.TagID = Convert.ToInt64(row["biTagID"]);
            }

            if (dt.Columns.Contains("vcTagNumber") && DBNull.Value != row["vcTagNumber"])
            {
                tag.TagNumber = row["vcTagNumber"].ToString();
            }

            if (dt.Columns.Contains("biSerialNumber") && DBNull.Value != row["biSerialNumber"])
            {
                tag.SerialNumber = Convert.ToInt64(row["biSerialNumber"]);
            }

            if (dt.Columns.Contains("biReceivedBoxID") && DBNull.Value != row["biReceivedBoxID"])
            {
                tag.ReceivedBoxID = Convert.ToInt64(row["biReceivedBoxID"]);
            }

            if (dt.Columns.Contains("biIssuedBoxID") && DBNull.Value != row["biIssuedBoxID"])
            {
                tag.IssuedBoxID = Convert.ToInt16(row["biIssuedBoxID"]);
            }

            if (dt.Columns.Contains("tiTagStatusID") && DBNull.Value != row["tiTagStatusID"])
            {
                tag.StatusID = Convert.ToInt16(row["tiTagStatusID"]);
            }

            if (dt.Columns.Contains("vcTagStatus") && DBNull.Value != row["vcTagStatus"])
            {
                tag.Status = row["vcTagStatus"].ToString();
            }

            if (dt.Columns.Contains("iPin") && DBNull.Value != row["iPin"])
            {
                tag.PIN = Convert.ToInt32(row["iPin"]);
            }

            if (dt.Columns.Contains("bCostTag") && DBNull.Value != row["bCostTag"])
            {
                tag.CostTag = Convert.ToBoolean(row["bCostTag"]);
            }

            tag.ReceivedBox = new ReceivedBox();


            if (dt.Columns.Contains("biReceivedBoxID") && DBNull.Value != row["biReceivedBoxID"])
            {
                tag.ReceivedBox.ReceivedBoxID = Convert.ToInt64(row["biReceivedBoxID"]);
            }

            if (dt.Columns.Contains("biBoxShipmentID") && DBNull.Value != row["biBoxShipmentID"])
            {
                tag.ReceivedBox.ShipmentID = Convert.ToInt64(row["biBoxShipmentID"]);
            }

            if (dt.Columns.Contains("tiBoxTypeID") && DBNull.Value != row["tiBoxTypeID"])
            {
                tag.ReceivedBox.BoxTypeID = Convert.ToInt16(row["tiBoxTypeID"]);
            }


            if (dt.Columns.Contains("biStartTag") && DBNull.Value != row["biStartTag"])
            {
                tag.ReceivedBox.StartTag = Convert.ToInt64(row["biStartTag"]);
            }

            if (dt.Columns.Contains("biEndTag") && DBNull.Value != row["biEndTag"])
            {
                tag.ReceivedBox.EndTag = Convert.ToInt64(row["biEndTag"]);
            }

            if (dt.Columns.Contains("vcBoxStatus") && DBNull.Value != row["vcBoxStatus"])
            {
                tag.ReceivedBox.Status = row["vcBoxStatus"].ToString();
            }

            if (dt.Columns.Contains("tiReceivedBoxStatusID") && DBNull.Value != row["tiReceivedBoxStatusID"])
            {
                tag.ReceivedBox.StatusID = Convert.ToInt16(row["tiReceivedBoxStatusID"]);
            }

            tag.ReceivedBox.Shipment = new Shipment();

            if (dt.Columns.Contains("biShipmentID") && DBNull.Value != row["biShipmentID"])
            {
                tag.ReceivedBox.Shipment.ShipmentID = Convert.ToInt64(row["biShipmentID"]);
            }

            if (dt.Columns.Contains("vcShipmentName") && DBNull.Value != row["vcShipmentName"])
            {
                tag.ReceivedBox.Shipment.ShipmentName = row["vcShipmentName"].ToString();
            }


            if (dt.Columns.Contains("vcPurchaseOrder") && DBNull.Value != row["vcPurchaseOrder"])
            {
                tag.ReceivedBox.Shipment.PurchaseOrder = row["vcPurchaseOrder"].ToString();
            }

            if (dt.Columns.Contains("dtOrderDate") && DBNull.Value != row["dtOrderDate"])
            {
                tag.ReceivedBox.Shipment.OrderDate = Convert.ToDateTime(row["dtOrderDate"]);
            }

            if (dt.Columns.Contains("dtShipmentDate") && DBNull.Value != row["dtShipmentDate"])
            {
                tag.ReceivedBox.Shipment.ShipmentDate = Convert.ToDateTime(row["dtShipmentDate"]);
            }

            if (dt.Columns.Contains("dtDeliveryDate") && DBNull.Value != row["dtDeliveryDate"])
            {
                tag.ReceivedBox.Shipment.DeliveryDate = Convert.ToDateTime(row["dtDeliveryDate"]);
            }

            if (dt.Columns.Contains("vcShipmentStatus") && DBNull.Value != row["vcShipmentStatus"])
            {
                tag.ReceivedBox.Shipment.Status = row["vcShipmentStatus"].ToString();
            }

            if (dt.Columns.Contains("tiShipmentStatusID") && DBNull.Value != row["tiShipmentStatusID"])
            {
                tag.ReceivedBox.Shipment.StatusID = Convert.ToInt16(row["tiShipmentStatusID"]);
            }

            return tag;
        }

        public static Dictionary<string, List<KeyValuePair<short, string>>> ToLookup(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            var lookups = new Dictionary<string, List<KeyValuePair<short, string>>>();

            #region tags status lookup
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiTagStatusID"]);
                    string status = row["vcTagStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.TagStatus.ToString(), tagsStatus);
            }
            #endregion

            #region received box status
            if (dataSet.Tables[1].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiReceivedBoxStatusID"]);
                    string status = row["vcBoxStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.ReceivedBoxStatus.ToString(), tagsStatus);
            }
            #endregion

            #region issued box status
            if (dataSet.Tables[2].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[2].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiIssuedBoxStatusID"]);
                    string status = row["vcIssuedBoxStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.IssuedBoxStatus.ToString(), tagsStatus);
            }
            #endregion


            #region visual check status 
            if (dataSet.Tables[3].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[3].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiVisualCheckStatusID"]);
                    string status = row["vcVisualCheckStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.VisualCheckStatus.ToString(), tagsStatus);
            }
            #endregion

            #region rfid check status
            if (dataSet.Tables[4].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[4].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiRFIDCheckStatusID"]);
                    string status = row["vcRFIDCheckStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.RFIDCheckStatus.ToString(), tagsStatus);
            }
            #endregion


            #region receive box type 
            if (dataSet.Tables[5].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[5].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiBoxTypeID"]);
                    string status = row["vcBoxType"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.ReceivedBoxType.ToString(), tagsStatus);
            }
            #endregion


            #region receive box type 
            if (dataSet.Tables[6].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[6].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiKitRFIDCheckStatusID"]);
                    string status = row["tiKitRFIDCheckStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.KitRFIDCheckStatus.ToString(), tagsStatus);
            }
            #endregion

            #region receive box type 
            if (dataSet.Tables[7].Rows.Count > 0)
            {
                var tagsStatus = new List<KeyValuePair<short, string>>();

                foreach (DataRow row in dataSet.Tables[7].Rows)
                {
                    short tagStatusID = Convert.ToInt16(row["tiVisualCheckStatusID"]);
                    string status = row["vcVisualCheckStatus"].ToString();

                    tagsStatus.Add(new KeyValuePair<short, string>(tagStatusID, status));
                }

                lookups.Add(TagInventory.Common.Enums.Lookup.KitVisualCheckStatus.ToString(), tagsStatus);
            }
            #endregion

            return lookups;

        }


        public static DistributorAndTypes ToDistributors(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                throw new ArgumentNullException(nameof(ds));
            }

            DistributorAndTypes result = new DistributorAndTypes();

            if (ds.Tables.Count > 0)
            {
                result.Distributors = new List<Distributor>();

                var tbDistributors = ds.Tables[0];

                foreach (DataRow row in tbDistributors.Rows)
                {
                    Distributor distributor = new Distributor();

                    distributor.DistributorID = Convert.ToInt16(row["siDistributorID"]);

                    if (tbDistributors.Columns.Contains("vcDistributorName") && row["vcDistributorName"] != DBNull.Value)
                    {
                        distributor.DistributorName = row["vcDistributorName"].ToString();
                    }

                    if (tbDistributors.Columns.Contains("tiPaymentLocationTypeID") && row["tiPaymentLocationTypeID"] != DBNull.Value)
                    {
                        distributor.PaymentLocationTypeID = Convert.ToByte(row["tiPaymentLocationTypeID"]);
                    }

                    if (tbDistributors.Columns.Contains("bActive") && row["bActive"] != DBNull.Value)
                    {
                        distributor.Active = Convert.ToBoolean(row["bActive"]);
                    }

                    if (tbDistributors.Columns.Contains("siDistributorTypeId") && row["siDistributorTypeId"] != DBNull.Value)
                    {
                        distributor.DistributorTypeId = Convert.ToInt16(row["siDistributorTypeId"]);
                    }

                    if (tbDistributors.Columns.Contains("vcDistributorTypeName") && row["vcDistributorTypeName"] != DBNull.Value)
                    {
                        distributor.DistributorTypeName = row["vcDistributorTypeName"].ToString();
                    }

                    if (tbDistributors.Columns.Contains("vcDistributorTypeDesc") && row["vcDistributorTypeDesc"] != DBNull.Value)
                    {
                        distributor.DistributorTypeDesc = row["vcDistributorTypeDesc"].ToString();
                    }

                    result.Distributors.Add(distributor);
                }
            }




            if (ds.Tables.Count > 1)
            {

                result.DistributorTypes = new List<DistributorType>();

                var tbDistributorType = ds.Tables[1];

                foreach (DataRow row in tbDistributorType.Rows)
                {
                    DistributorType distType = new DistributorType();

                    distType.DistributorTypeID = Convert.ToInt16(row["siDistributorTypeId"]);

                    if (tbDistributorType.Columns.Contains("vcDistributorTypeName") && row["vcDistributorTypeName"] != DBNull.Value)
                    {
                        distType.DistributorTypeName = row["vcDistributorTypeName"].ToString();
                    }

                    if (tbDistributorType.Columns.Contains("vcDistributorTypeDesc") && row["vcDistributorTypeDesc"] != DBNull.Value)
                    {
                        distType.DistributorTypeDesc = row["vcDistributorTypeDesc"].ToString();
                    }

                    result.DistributorTypes.Add(distType);
                }


            }

            return result;
        }


        public static Page<IssuedBox> ToIssuedBoxes(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            Page<IssuedBox> page = new Page<IssuedBox>() { Data = new List<IssuedBox>() };

            var table = dataSet.Tables[0];

            var response = GetIssuedBoxList(table);

            page.Data.AddRange(response);


            var searchCount = 0;
            var totalCount = 0;

            if (dataSet.Tables[1].Columns.Contains("SearchCount") && DBNull.Value != dataSet.Tables[1].Rows[0]["SearchCount"])
                searchCount = Convert.ToInt32(dataSet.Tables[1].Rows[0]["SearchCount"]);

            if (dataSet.Tables[2].Columns.Contains("TotalCount") && DBNull.Value != dataSet.Tables[2].Rows[0]["TotalCount"])
                totalCount = Convert.ToInt32(dataSet.Tables[2].Rows[0]["TotalCount"]);

            page.SearchCount = Convert.ToInt32(searchCount);

            page.TotalCount = Convert.ToInt32(totalCount);

            var tags = GetTags(dataSet.Tables[3].Copy());

            var join = page.Data.Join(tags, ib => ib.IssuedBoxID, t => t.IssuedBoxID, (ib, t) => new { IB = ib, T = t }).ToList();

            foreach (IssuedBox box in page.Data)
            {
                var boxTags = join.FindAll(r => r.T.IssuedBoxID == box.IssuedBoxID).Select(r => r.T).ToList();

                if (boxTags != null && boxTags.Count > 0)
                {
                    box.Tags = new List<Tag>();
                    box.Tags.AddRange(boxTags);
                }

            }

            return page;
        }

        private static List<IssuedBox> GetIssuedBoxList(DataTable table)
        {

            List<IssuedBox> response = new List<IssuedBox>();

            foreach (DataRow row in table.Rows)
            {
                IssuedBox issuedBox = new IssuedBox();

                issuedBox.IssuedBoxID = Convert.ToInt16(row["biIssuedBoxID"]);

                if (table.Columns.Contains("siQuantity"))
                {
                    issuedBox.Quantity = Convert.ToInt16(row["siQuantity"]);
                }

                if (table.Columns.Contains("biReceivedBoxID"))
                {
                    issuedBox.ReceivedBoxID = Convert.ToInt64(row["biReceivedBoxID"]);
                }


                if (table.Columns.Contains("dtSendDate") && DBNull.Value != row["dtSendDate"])
                {
                    issuedBox.SendDate = Convert.ToDateTime(row["dtSendDate"]);
                }

                if (table.Columns.Contains("dtReceivedDate") && DBNull.Value != row["dtReceivedDate"])
                {
                    issuedBox.ReceivedDate = Convert.ToDateTime(row["dtReceivedDate"]);
                }

                if (table.Columns.Contains("siDistributor") && DBNull.Value != row["siDistributor"])
                {
                    issuedBox.DistributorID = Convert.ToInt16(row["siDistributor"]);
                }

                if (table.Columns.Contains("vcDistributorName") && DBNull.Value != row["vcDistributorName"])
                {
                    issuedBox.Distributor = row["vcDistributorName"].ToString();
                }



                if (table.Columns.Contains("dtIssuedDate") && DBNull.Value != row["dtIssuedDate"])
                {
                    issuedBox.IssuedDate = Convert.ToDateTime(row["dtIssuedDate"]);
                }

                if (table.Columns.Contains("tiIssuedBoxStatusID"))
                {
                    issuedBox.StatusID = Convert.ToInt16(row["tiIssuedBoxStatusID"]);
                }

                if (table.Columns.Contains("vcIssuedBoxStatus"))
                {
                    issuedBox.Status = row["vcIssuedBoxStatus"].ToString();
                }

                if (table.Columns.Contains("tiIssuedBoxStatusID"))
                {
                    issuedBox.StatusID = Convert.ToInt16(row["tiIssuedBoxStatusID"]);
                }

                if (table.Columns.Contains("bInitialAssigned"))
                {
                    issuedBox.InitialAssigned = Convert.ToBoolean(row["bInitialAssigned"]);
                }

                if (table.Columns.Contains("dtCreatedTime"))
                {

                    issuedBox.CreatedDate = Convert.ToDateTime(row["dtCreatedTime"]);
                }

                if (table.Columns.Contains("iCreatedUserID"))
                {

                    issuedBox.CreatedUserID = Convert.ToInt32(row["iCreatedUserID"]);
                }

                if (table.Columns.Contains("iUpdUserID") && DBNull.Value != row["iUpdUserID"])
                {
                    issuedBox.UpdateUserID = Convert.ToInt32(row["iUpdUserID"]);
                }

                if (table.Columns.Contains("dtUpdTime") && DBNull.Value != row["dtUpdTime"])
                {
                    issuedBox.UpdatedDate = Convert.ToDateTime(row["dtUpdTime"]);
                }

                if (table.Columns.Contains("biShipmentID") && DBNull.Value != row["biShipmentID"])
                {
                    issuedBox.ShipmentID = Convert.ToInt64(row["biShipmentID"]);
                }

                if (table.Columns.Contains("dtUpdDate") && DBNull.Value != row["dtUpdDate"])
                {
                    issuedBox.UpdatedDatePart = Convert.ToDateTime(row["dtUpdDate"]);
                }

                if (table.Columns.Contains("vcFileName") && DBNull.Value != row["vcFileName"])
                {
                    issuedBox.ShipmentFileName = row["vcFileName"].ToString();
                }

                if (table.Columns.Contains("biTagShipmentHdrID") && DBNull.Value != row["biTagShipmentHdrID"])
                {
                    issuedBox.ShipmentFileID = Convert.ToInt64(row["biTagShipmentHdrID"]);
                }


                response.Add(issuedBox);
            }

            return response;
        }


        public static SerialList ToSerialList(this DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            var dt = dataSet.Tables[0];

            SerialList sList = new SerialList();

            sList.SerialListHDR = new List<SerialListHDR>();


            for (int c = 0; c < dt.Rows.Count; c++)
            {

                var issuedBoxID = Convert.ToInt64(dt.Rows[c]["IssuedBoxID"]);

                var existingHDR = sList.SerialListHDR.Find(s => s.IssuedBoxID == issuedBoxID);

                if (existingHDR == null)
                {
                    var from = Convert.ToInt64(dt.Rows[c]["From"]);

                    var to = Convert.ToInt64(dt.Rows[c]["To"]);

                    var qty = Convert.ToInt32(dt.Rows[c]["Qty"]);

                    SerialListHDR hdr = new SerialListHDR();

                    hdr.IssuedBoxID = issuedBoxID;

                    hdr.SerialRange = new List<SerialNumberRange>();

                    hdr.SerialRange.Add(new SerialNumberRange() { From = from, To = to, Qty = qty });

                    sList.SerialListHDR.Add(hdr);

                }
                else
                {
                    var from = Convert.ToInt64(dt.Rows[c]["From"]);

                    var to = Convert.ToInt64(dt.Rows[c]["To"]);

                    var qty = Convert.ToInt32(dt.Rows[c]["Qty"]);

                    existingHDR.SerialRange.Add(new SerialNumberRange() { From = from, To = to, Qty = qty });
                }

            }
            return sList;
        }

        public static List<ShipmentFile> ToExportPackage(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                throw new ArgumentNullException(nameof(ds));
            }

            var dtShipmentFiles = ds.Tables[0];

            List<ShipmentFile> shipmentFiles = new List<ShipmentFile>();

            foreach (DataRow row in dtShipmentFiles.Rows)
            {
                ShipmentFile file = new ShipmentFile();

                if (dtShipmentFiles.Columns.Contains("biTagShipmentHdrID"))
                {
                    file.ShipmentFileID = Convert.ToInt64(row["biTagShipmentHdrID"]);
                }

                if (dtShipmentFiles.Columns.Contains("biShipmentID"))
                {
                    file.ShipmentID = Convert.ToInt16(row["biShipmentID"]);
                }

                //if (dtShipmentFiles.Columns.Contains("dcCostPerTag") && DBNull.Value != row["dcCostPerTag"])
                //{
                //    file.cos = Convert.ToDateTime(row["dcCostPerTag"]);
                //}


                if (dtShipmentFiles.Columns.Contains("iOrderQuantity") && DBNull.Value != row["iOrderQuantity"])
                {
                    file.OrderQuantity = Convert.ToInt16(row["iOrderQuantity"]);
                }

                if (dtShipmentFiles.Columns.Contains("vcCaseNumber") && DBNull.Value != row["vcCaseNumber"])
                {
                    file.CaseNumber = row["vcCaseNumber"].ToString();
                }

                if (dtShipmentFiles.Columns.Contains("biCaseID") && DBNull.Value != row["biCaseID"])
                {
                    file.CaseID = Convert.ToInt64(row["biCaseID"]);
                }


                if (dtShipmentFiles.Columns.Contains("vcFileName") && DBNull.Value != row["vcFileName"])
                {
                    file.FileName = row["vcFileName"].ToString();
                }

                if (dtShipmentFiles.Columns.Contains("vcPartNumber") && DBNull.Value != row["vcPartNumber"])
                {
                    file.PartNumber = row["vcPartNumber"].ToString();
                }

                if (dtShipmentFiles.Columns.Contains("vcSalesOrderNumber") && DBNull.Value != row["vcSalesOrderNumber"])
                {
                    file.SalesOrderNumber = row["vcSalesOrderNumber"].ToString();
                }

                if (dtShipmentFiles.Columns.Contains("dtCreatedTime") && DBNull.Value != row["dtCreatedTime"])
                {
                    file.CreatedDate = Convert.ToDateTime(row["dtCreatedTime"]);
                }

                if (dtShipmentFiles.Columns.Contains("dtUpdTime") && DBNull.Value != row["dtUpdTime"])
                {
                    file.UpdatedDate = Convert.ToDateTime(row["dtUpdTime"]);
                }

                if (dtShipmentFiles.Columns.Contains("iCreatedUserID") && DBNull.Value != row["iCreatedUserID"])
                {
                    file.CreatedUserID = Convert.ToInt32(row["iCreatedUserID"]);
                }

                if (dtShipmentFiles.Columns.Contains("iUpdUserID") && DBNull.Value != row["iUpdUserID"])
                {
                    file.UpdatedUserID = Convert.ToInt32(row["iUpdUserID"]);
                }

                shipmentFiles.Add(file);
            }

            if (ds.Tables.Count < 2)
                throw new Exception("Expecting issued boxes from store procedure to process");

            List<ReceivedBox> receivedBoxList = GetReceivedBox(ds.Tables[1]);


            if (ds.Tables.Count < 3)
                throw new Exception("Expecting tags from store procedure to process");

            List<Tag> tags = GetTags(ds.Tables[2]);

            foreach (ShipmentFile file in shipmentFiles)
            {
                file.ReceivedBoxes = new List<ReceivedBox>();

                file.ReceivedBoxes.AddRange(receivedBoxList.FindAll(bx => bx.ShipmentFileID == file.ShipmentFileID));

                foreach (ReceivedBox receivedBox in file.ReceivedBoxes)
                {
                    receivedBox.Tags = new List<Tag>();

                    receivedBox.Tags.AddRange(tags.FindAll(t => t.IssuedBoxID == receivedBox.IssuedBoxID));
                }
            }

            return shipmentFiles;

        }

    }
}
