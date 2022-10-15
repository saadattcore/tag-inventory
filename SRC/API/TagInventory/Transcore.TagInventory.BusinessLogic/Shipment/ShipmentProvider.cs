using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.DataAccess;
using System.Data;
using Transcore.TagInventory.Entity.Model;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;

namespace Transcore.TagInventory.BusinessLogic
{
    public class ShipmentProvider : IShipmentProvider
    {
        private IShipmentRepository _repository;

        public ShipmentProvider(IShipmentRepository repository)
        {
            _repository = repository;
        }

        public long AddShipment(Shipment shipment)
        {
            return _repository.Add(shipment);
        }

        public Page<Shipment> GetShipment(ShipmentSearch searchOptions, int pageSize, int pageNumber)
        {
            var shipments = _repository.GetShipment(searchOptions, pageSize, pageNumber);
            return shipments;
        }

        public void Update(Shipment shipment)
        {
            _repository.Update(shipment);
        }
    }
}
