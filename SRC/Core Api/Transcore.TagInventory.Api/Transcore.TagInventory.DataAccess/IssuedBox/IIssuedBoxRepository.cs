using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.DataAccess
{
    public interface IIssuedBoxRepository
    {
        long Add(IssuedBox issuedBox);

        long UpdateBoxAndTags(IssuedBox issuedBox, bool updateIssuedBoxKits);

        void Update(IssuedBox issuedBox);

        Page<IssuedBox> GetIssuedBox(IssuedBoxSearch searchOptions, int pageSize, int pageNumber);

        DataSet GetReportData(string issuedBoxIDList);

        SerialList GetSerialListData(string issuedBoxIDList, long? shipmentID);

        void UpdateBoxesStatus(List<IssuedBox> boxList);

        List<IssuedBoxActivityHistory> GetIssuedBoxHistory(long issuedBoxID);

    }
}
