using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Inventory.BusinessLogic
{
    public interface IReceivedBoxProvider
    {
        Page<ReceivedBox> ImportShipmentFileToDB(string uploadedFilesPath,long shipmentID, byte boxTypeID);

        Page<ReceivedBox> GetReceivedBox(ReceivedBoxSearch searchOptions, int pageSize, int pageNumber);

        void UpdateStatus(ReceivedBoxUpdate receivedBoxUpd);

        ReceivedBox UpdateScannedBox(ScannedReceivedBoxUpdate boxScanTags);

        void GenerateShipmentPackage(long shipmentID);

    }
}
