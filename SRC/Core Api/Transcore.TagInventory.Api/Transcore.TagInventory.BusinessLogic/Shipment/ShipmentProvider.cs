
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
using Inventory.Services.Common;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Resources;
using Transcore.TagInventory.Common.Enums;

namespace Transcore.TagInventory.BusinessLogic
{
    public class ShipmentProvider : IShipmentProvider
    {
        private readonly IShipmentRepository _repository;
        private readonly IConfiguration _appSettings;

        public ShipmentProvider(IShipmentRepository repository, IConfiguration appSettings)
        {
            _repository = repository;
            _appSettings = appSettings;
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
