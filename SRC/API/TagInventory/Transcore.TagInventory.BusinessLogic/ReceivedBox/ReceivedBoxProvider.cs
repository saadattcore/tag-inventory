using Inventory.Services.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.DataAccess;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Inventory.BusinessLogic
{
    public class ReceivedBoxProvider : IReceivedBoxProvider
    {
        private readonly IReceivedBoxRepository _repository;

        public ReceivedBoxProvider(IReceivedBoxRepository repository)
        {
            _repository = repository;
        }

        public Page<ReceivedBox> GetReceivedBox(ReceivedBoxSearch searchOptions, int pageSize, int pageNumber)
        {
            return _repository.GetReceivedBox(searchOptions, pageSize, pageNumber);
        }

        public Page<ReceivedBox> Import(Dictionary<string, string> files, long shipmentID, short boxTypeID)
        {
            var importedFiles = FileHandler.ParseImportFiles(files, shipmentID, boxTypeID);

            return _repository.Import(importedFiles);

            //var response = $"Sucessfully imported {importedFiles.Count}";

            //return response;
        }

        /// <summary>
        /// Parse string files content and build 1 - File, 2 - RecievedBox and 3 - Tag.
        /// </summary>
        private List<TagShipmentHeader> FilesToEntity(Dictionary<string, string> files)
        {
            return null;
        }

        public void UpdateStatus(ReceivedBoxUpdate receivedBoxUpd)
        {
            _repository.UpdateStatus(receivedBoxUpd);
        }

        public ReceivedBox UpdateScannedBox(ScannedReceivedBoxUpdate boxScanTags)
        {
            if (boxScanTags == null)
                throw new ArgumentNullException(nameof(boxScanTags));

            var additionalTags = boxScanTags.ScanTags.Where(t => !t.IsImported).ToList();

            additionalTags.ForEach(tag => tag.TagID = Convert.ToInt64(tag.TagNumber, 16));

            return _repository.UpdateScannedBox(boxScanTags);
        }
    }
}
