using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.BusinessLogic
{
    public interface IIssuedBoxProvider
    {
        long Add(IssuedBox issuedBox);

        long UpdateBoxAndTags(IssuedBox issuedBox, bool updateIssuedBoxKits);

        void Update(IssuedBox issuedBox);

        Page<IssuedBox> GetIssuedBox(IssuedBoxSearch searchOptions, int pageSize, int pageNumber);

        void PrintBoxLabel(string issuedBoxIDList);

        byte[] ExportLabelToPDF(string issuedBoxIDList);

        byte[] DownLoadSerialList(string issuedBoxIDList, long? shipmentID);

        void UpdateBoxesStatus(List<IssuedBox> boxList);

        List<IssuedBoxActivityHistory> GetIssuedBoxHistory(long issuedBoxID);
    }
}
