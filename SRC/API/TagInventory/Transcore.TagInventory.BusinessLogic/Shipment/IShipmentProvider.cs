using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.BusinessLogic
{
    public interface IShipmentProvider
    {
        long AddShipment(Shipment shipment);

        void Update(Shipment shipment);

        Page<Shipment> GetShipment(ShipmentSearch seacrhOptions,int pageSize, int pageNumber);      

    }
}
