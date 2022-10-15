using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transcore.TagInventory.Common.Reporting
{
    public class LabelPrinter : IDisposable, ILabelPrinter
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        // Create a local report for Report.rdlc, load the data,
        //    export the report to an .emf file, and print it.
        public void PrintLabel(DataTable dt, string totalBoxTags)
        {
            dt.Columns.Add(new System.Data.DataColumn("BarCode", typeof(byte[])));

            BarcodeLib.Barcode barCode = new BarcodeLib.Barcode();

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                var issuedBoxID = Convert.ToInt64(dt.Rows[index]["IssuedBoxID"]).ToString("D2");

                Image image = barCode.Encode(BarcodeLib.TYPE.CODE39, issuedBoxID, Color.Black, Color.White);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);

                    dt.Rows[index]["BarCode"] = ms.ToArray();
                }
            }

            string rootPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            int lastIndex = rootPath.LastIndexOf("\\");

            string subPath = rootPath.Substring(0, lastIndex);

            subPath = subPath + "\\Transcore.TagInventory.Common\\Reporting\\TagInventory.rdlc";

            LocalReport report = new LocalReport();

            report.ReportPath = subPath;

            report.DataSources.Add(new ReportDataSource("TagInventoryDS", dt));

            //        ReportParameter[] reportParams = new ReportParameter[]
            //{
            //                new ReportParameter("IssuedBoxID", "0"),

            //                new ReportParameter("TotalTags", "0")
            //};
            //        report.SetParameters(reportParams);

            Export(report);

            Print();
        }

        public byte[] ExportLabelToPDF(DataTable dt)
        {
            dt.Columns.Add(new System.Data.DataColumn("BarCode", typeof(byte[])));

            BarcodeLib.Barcode barCode = new BarcodeLib.Barcode();

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                var issuedBoxID = Convert.ToInt64(dt.Rows[index]["IssuedBoxID"]).ToString("D2");

                Image image = barCode.Encode(BarcodeLib.TYPE.CODE39, issuedBoxID, Color.Black, Color.White);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);

                    dt.Rows[index]["BarCode"] = ms.ToArray();
                }
            }     

            string reportPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Reporting", "TagInventory.rdlc");   

            LocalReport report = new LocalReport();

            report.ReportPath = reportPath;

            report.DataSources.Add(new ReportDataSource("TagInventoryDS", dt));

            string deviceInfo =
           @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";


            return report.Render("PDF", deviceInfo);            

        }


        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
