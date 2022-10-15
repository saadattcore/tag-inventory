using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Core;

namespace Inventory.Services.Common
{
    public static class Extensions
    {
        public static TagShipmentHeader ToImportFile(this string fileInfo)
        {
            var importFile = new TagShipmentHeader();

            var keywords = fileInfo.Split('\t');

            foreach (var keyword in keywords)
            {
                if (keyword.ToLower().Contains("p/n"))
                {
                    importFile.PartNumber = keyword.Split()[1];
                }

                else if (keyword.ToLower().Contains("so/n"))
                {
                    importFile.SalesOrderNumber = keyword.Split()[1];
                }

                else if (keyword.ToLower().Contains("quantity"))
                {
                    importFile.OrderQuantity = Convert.ToInt16(keyword.Split()[1]);
                }

                else if (keyword.ToLower().Contains("case#"))
                {
                    var values = keyword.Split();
                    var tmpCaseNo = $"{values[1]} {values[2]}";

                    //importFile.CaseID = Convert.ToInt64(tmpCaseNo.Trim());
                    importFile.CaseNumber = tmpCaseNo;
                }

                else if (keyword.ToLower().Contains("filename"))
                {
                    var values = keyword.Split();


                    for (int index = 1; index < values.Length;index++)
                    {

                        if (!values[index].Contains("."))
                        {
                            importFile.FileName = importFile.FileName + values[index] + " ";
                        }
                        else
                        {
                            importFile.FileName = importFile.FileName + values[index];
                        }

                        //importFile.FileName = values[1] + " " + values[2];
                    }
                }

                else
                {
                    importFile.OrderProcessedDate = DateTime.ParseExact(keyword.Trim(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                }
            }


            return importFile;

        }


        public static ReceivedBox ToReceivedBox(this List<string> page)
        {
            var boxStr = page[0];
            page.RemoveAt(0);

            var tableHeader = page[1];
            page.RemoveAt(0);

            // First parse received box.

            var keywords = boxStr.Split('\t');
            var receivedBox = new ReceivedBox();
            receivedBox.Tags = new List<Tag>();


            foreach (var keyword in keywords)
            {
                if (keyword.Contains("BOX#"))
                {
                    receivedBox.ReceivedBoxID = Convert.ToInt64(keyword.Split()[1]);
                }

                else if (keyword.Contains("CASE#"))
                {
                    var values = keyword.Split();
                    var tmpCaseNo = values[1] + values[2];

                    receivedBox.CaseID = Convert.ToInt64(tmpCaseNo.Trim());
                }

                else if (keyword.Contains("Quantity"))
                {
                    receivedBox.Quantity = Convert.ToInt16(keyword.Split()[1]);
                }
            }

            // Second parse Tags



            foreach (var item in page)
            {
                if (string.IsNullOrEmpty(item)) continue;

                var rowStr = item.Split('\t');
                var tag = new Tag();

                tag.TagID = Convert.ToInt64(rowStr[0], 16);
                tag.TagNumber = rowStr[0];
                tag.SerialNumber = long.Parse(rowStr[1]);
                tag.ReceivedBoxID = receivedBox.ReceivedBoxID;
                tag.IsImported = true;
                tag.Marking = rowStr[2].Replace(" ",".");
                tag.Frame24 = rowStr[3];
                tag.Frame25 = rowStr[4];
                tag.Frame26 = rowStr[5];
                tag.Frame27 = rowStr[6];
                //tag.VisualCheck = false;
                //tag.OperationalCheck = false;
                //tag.RFIDCheck = false;
                tag.StatusID = 1; // 1 for imported
                tag.CreatedUserID = -1;
                tag.PIN = int.Parse((rowStr[rowStr.Length - 1]));

                receivedBox.Tags.Add(tag);

            }


            var sortedTags = receivedBox.Tags.OrderBy(tag => tag.SerialNumber).ToList();
            receivedBox.StartTag = sortedTags[0].SerialNumber;
            receivedBox.EndTag = sortedTags[sortedTags.Count - 1].SerialNumber;

            return receivedBox;
        }
    }
}
