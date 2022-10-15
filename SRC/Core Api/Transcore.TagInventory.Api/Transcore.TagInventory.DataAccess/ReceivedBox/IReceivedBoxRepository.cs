using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.DataAccess
{
    public interface IReceivedBoxRepository
    {
        Page<ReceivedBox> Import(List<ShipmentFile> fileCollection);

        Page<ReceivedBox> GetReceivedBox(ReceivedBoxSearch searchOptions, int pageSize, int pageNumber);

        void UpdateStatus(ReceivedBoxUpdate receivedBoxUpd);

        ReceivedBox UpdateScannedBox(ScannedReceivedBoxUpdate boxScanTags);

    }
}
