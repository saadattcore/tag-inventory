using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Reporting;
using Transcore.TagInventory.DataAccess;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.BusinessLogic
{
    public class IssuedBoxProvider : IIssuedBoxProvider
    {
        private readonly IIssuedBoxRepository _repository;

        private readonly ILabelPrinter _labelPrinter;

        public IssuedBoxProvider(IIssuedBoxRepository repository, ILabelPrinter labelPrinter)
        {
            _repository = repository;

            _labelPrinter = labelPrinter;
        }

        public Page<IssuedBox> GetIssuedBox(IssuedBoxSearch searchOptions, int pageSize, int pageNumber)
        {
            _repository.GetIssuedBox(searchOptions, 1, 1);

            return _repository.GetIssuedBox(searchOptions, pageSize, pageNumber);
        }

        public void Update(IssuedBox issuedBox)
        {
            if (issuedBox == null)
                throw new ArgumentException(nameof(issuedBox));

            _repository.Update(issuedBox);
        }

        public long Add(IssuedBox issuedBox)
        {
            if (issuedBox == null)
                throw new ArgumentException(nameof(issuedBox));

            return _repository.Add(issuedBox);
        }

        public long UpdateBoxAndTags(IssuedBox issuedBox, bool updateIssuedBoxKits)
        {
            if (issuedBox == null)
                throw new ArgumentException(nameof(issuedBox));

            return _repository.UpdateBoxAndTags(issuedBox, updateIssuedBoxKits);
        }

        public void PrintBoxLabel(string issuedBoxIDList)
        {
            var ds = _repository.GetReportData(issuedBoxIDList);

            if (ds.Tables.Count == 0)
            {
                throw new Exception("No report data found");
            }                       

            _labelPrinter.PrintLabel( ds.Tables[0], "0");

            _labelPrinter.Dispose();

        }

        public byte[] DownLoadSerialList(string issuedBoxIDList, long? shipmentID)
        {
            SerialList sList = _repository.GetSerialListData(issuedBoxIDList, shipmentID);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stream = new MemoryStream();

            using (ExcelPackage excel = new ExcelPackage(stream))
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[3, 1].Value = "Box Id";

                workSheet.Cells[3, 3].Value = "Ranges";

                workSheet.Cells[3, 6].Value = "Qty";

                workSheet.Cells[4, 3].Value = "From";

                workSheet.Cells[4, 4].Value = "To";

                workSheet.Cells[3, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                workSheet.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                workSheet.Cells[3, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                workSheet.Cells[3, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                workSheet.Cells[3, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                workSheet.Cells[3, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                workSheet.Cells[4, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                workSheet.Cells[4, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                workSheet.Cells[4, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                workSheet.Cells[4, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ExcelRange range = workSheet.Cells["A3:F3"];

                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                range.Dispose();

                int rowCounter = 5;

                for (int i = 0; i < sList.SerialListHDR.Count; i++)
                {
                    workSheet.Cells[rowCounter, 1].Value = sList.SerialListHDR[i].IssuedBoxID;

                    for (int j = 0; j < sList.SerialListHDR[i].SerialRange.Count; j++)
                    {
                        var rangeData = sList.SerialListHDR[i].SerialRange[j];

                        workSheet.Cells[rowCounter, 3].Value = rangeData.From;

                        workSheet.Cells[rowCounter, 4].Value = rangeData.To;

                        workSheet.Cells[rowCounter, 6].Value = rangeData.Qty;

                        rowCounter++;
                    }
                }
                excel.Save();

                var content = excel.GetAsByteArray();

                return content;
            }

        }

        public void UpdateBoxesStatus(List<IssuedBox> boxList)
        {
            _repository.UpdateBoxesStatus(boxList);
        }

        public byte[] ExportLabelToPDF(string issuedBoxIDList)
        {
            var ds = _repository.GetReportData(issuedBoxIDList);

            if (ds.Tables.Count == 0)
            {
                throw new Exception("No report data found");
            }

            return _labelPrinter.ExportLabelToPDF(ds.Tables[0]); 
        }

        public List<IssuedBoxActivityHistory> GetIssuedBoxHistory(long issuedBoxID)
        {
            return _repository.GetIssuedBoxHistory(issuedBoxID);
        }
    }
}
