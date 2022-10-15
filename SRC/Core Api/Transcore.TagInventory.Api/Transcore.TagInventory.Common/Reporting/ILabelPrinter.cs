using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Common.Reporting
{
    public interface ILabelPrinter
    {
        void PrintLabel(DataTable dt, string totalBoxTags);
        byte[] ExportLabelToPDF(DataTable dt);
        void Dispose();
    }
}
