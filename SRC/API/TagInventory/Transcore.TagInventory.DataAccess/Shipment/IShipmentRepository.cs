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
    public interface IShipmentRepository
    {
        long Add(Shipment shipment);

        void Update(Shipment shipment);

        Page<Shipment> GetShipment(ShipmentSearch shipment,int pageSize,int pageNumber);

    }
}
