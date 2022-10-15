using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.DataAccess.ExportPackage
{
    public interface IExportPackageRepository
    {
        List<ShipmentFile> GetExportPackage(long shipmentId, int batchSize);
    }
}
