using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.DataAccess.ExportPackage;
using Transcore.TagInventory.Entity.Core;
using System.Resources;
using Transcore.TagInventory.Common.Enums;
using Inventory.Services.Common;
using System.IO.Compression;

namespace Transcore.TagInventory.BusinessLogic
{
    public class ExportPackageProvider : IExportPackageProvider
    {
        private string _importFolder;
        private string _importShipmentFolder;
        private string _exportPackageFolder;
        private string _noCostFolder;
        private string _costFolder;
        private string _pricedTagsScript;
        private string _freeTagsScript;
        private string _exportPakageShipmentFolder;
        private string _exportPackageZip;
        private List<ShipmentFile> _dbShipmentFiles;
        private List<ShipmentFile> _uploadedShipmentFiles;
        private readonly IConfiguration _appSettings;
        private readonly IExportPackageRepository _repository;

        public ExportPackageProvider(IConfiguration appSettings, IExportPackageRepository repository)
        {
            _appSettings = appSettings;

            _repository = repository;
        }

        public void InitializeMembers(long shipmentID, bool containsFreeTags)
        {
            _importFolder = _appSettings.GetValue<string>("ImportFolder");

            _importFolder = _appSettings.GetValue<string>("ImportFolder");

            _importShipmentFolder = Path.Combine(_importFolder, shipmentID.ToString()); // _appSettings.GetValue<string>("ShipmentFolder").Replace("{shipmentID}", shipmentID.ToString());

            _exportPackageFolder = _appSettings.GetValue<string>("ExportFolder");

            _exportPakageShipmentFolder = Path.Combine(_exportPackageFolder, shipmentID.ToString());

            _exportPackageZip = Path.Combine(_exportPackageFolder, shipmentID.ToString() + "_shipment package.zip");

            _noCostFolder = Path.Combine(_exportPackageFolder, shipmentID.ToString(), "No Cost Tags"); // _appSettings.GetValue<string>("NoCostTagFolder").Replace("{shipmentID}", shipmentID.ToString());

            _costFolder = Path.Combine(_exportPackageFolder, shipmentID.ToString(), "Cost Tags"); // _appSettings.GetValue<string>("CostTagFolder").Replace("{shipmentID}", shipmentID.ToString());

            _pricedTagsScript = _appSettings.GetValue<string>("PricedTagsScript");

            _freeTagsScript = _appSettings.GetValue<string>("FreeTagsScript");

            int batchSize = containsFreeTags ? _appSettings.GetValue<int>("BatchSize") : 0;

            _dbShipmentFiles = _repository.GetExportPackage(shipmentID, batchSize);

            _uploadedShipmentFiles = FileHandler.ParseUploadedFiles(_importShipmentFolder, shipmentID, 0);
        }

        public byte[] GetShipmentExportPackage(long shipmentID, bool containsFreeTags)
        {
            return null;

            InitializeMembers(shipmentID, containsFreeTags);

            //if (Directory.Exists(_exportPakageShipmentFolder))
            //{
            //    Directory.Delete(_exportPakageShipmentFolder, true);
            //}

            AssignPackageTypeToFile();

            List<string> exportFile = new List<string>();

            var files = _dbShipmentFiles.Join(_uploadedShipmentFiles, dbFile => dbFile.FileName, diskFile => diskFile.FileName, (dbFile, diskFile) => new { DBFile = dbFile, DiskFile = diskFile });

            foreach (var dbUpdFilePair in files)
            {
                dbUpdFilePair.DiskFile.PackageType = dbUpdFilePair.DBFile.PackageType;

                var rbListToWrite = dbUpdFilePair.DiskFile.ReceivedBoxes.Join(dbUpdFilePair.DBFile.ReceivedBoxes, rb => rb.ReceivedBoxID, ib => ib.ReceivedBoxID, (fileRB, dbRB) => new { FilesRB = fileRB, DbRB = dbRB });

                foreach (var rbToWrite in rbListToWrite)
                {
                    Action<Tag> assignProperties = (Tag targetTag) =>
                    {
                        Tag sourceTag = rbToWrite.DbRB.Tags.Find(DTag => DTag.TagID == targetTag.TagID);
                        targetTag.CostTag = sourceTag.CostTag;
                        targetTag.StatusID = sourceTag.StatusID;
                    };

                    rbToWrite.FilesRB.Tags.ForEach(assignProperties);

                    var tagsWhichMovedFromAnotherBox = rbToWrite.DbRB.Tags.Where(dbTag => !rbToWrite.FilesRB.Tags.Exists(fileTag => fileTag.TagID == dbTag.TagID));

                    if (tagsWhichMovedFromAnotherBox.Any())
                    {
                        var tags = LookupReplacedTagInFiles(tagsWhichMovedFromAnotherBox.ToList());

                        rbToWrite.FilesRB.Tags.AddRange(tags);

                        rbToWrite.FilesRB.Tags.RemoveAll(t => t.StatusID == (short)TagStatus.Defective);

                    }
                }
            }

            files.Select(pair => pair.DiskFile).ToList().WriteFilesToCostNoCost(_costFolder, _noCostFolder);

            ResourceManager resx = new ResourceManager("Transcore.TagInventory.BusinessLogic.Resource", System.Reflection.Assembly.GetExecutingAssembly());

            //if (Directory.Exists(_noCostFolder) && Directory.GetFiles(_noCostFolder, "*.txt").Length > 0)
            //{
            //    WriteSqlScript(resx.GetString("NoCostTagsScript"), _noCostFolder, _freeTagsScript);
            //}

            //if (Directory.Exists(_costFolder) && Directory.GetFiles(_costFolder, "*.txt").Length > 0)
            //{
            //    WriteSqlScript(resx.GetString("CostTagsScript"), _costFolder, _pricedTagsScript);
            //}

            if (File.Exists(_exportPackageZip))
            {
                File.Delete(_exportPackageZip);
            }

            ZipFile.CreateFromDirectory(_exportPakageShipmentFolder, _exportPackageZip);

            return File.ReadAllBytes(_exportPackageZip);
        }


        private List<Tag> LookupReplacedTagInFiles(List<Tag> dbTagsMovedFromSpare)
        {

            List<Tag> uploadedTags = new List<Tag>();

            foreach (ShipmentFile file in _uploadedShipmentFiles)
            {
                foreach (ReceivedBox rb in file.ReceivedBoxes)
                {
                    var rr = (from t in rb.Tags
                              join r in dbTagsMovedFromSpare
                              on t.TagID equals r.TagID
                              select t
                            ).ToList();

                    if (rr.Count > 0)
                        uploadedTags.AddRange(rr.ToList());
                }
            }

            return uploadedTags;

        }
        private void AssignPackageTypeToFile()
        {
            List<Tag> allTags = new List<Tag>();

            foreach (ShipmentFile file in _dbShipmentFiles)
            {
                file.ReceivedBoxes.ForEach(t => allTags.AddRange(t.Tags));

                if (allTags.Count == allTags.Where(t => t.CostTag).Count())
                    file.PackageType = Common.Enums.ExportPackageType.CostTags;

                else if (allTags.Count == allTags.Where(t => !t.CostTag).Count())
                    file.PackageType = Common.Enums.ExportPackageType.NoCostTags;

                else
                    file.PackageType = Common.Enums.ExportPackageType.PartialTags;


                allTags.Clear();
            }

        }


        private void WriteSqlScript(string script, string filesSourceFolder, string scriptFile)
        {
            StringBuilder sb = new StringBuilder();

            List<string> files = Directory.GetFiles(filesSourceFolder, "*.txt").ToList();

            foreach (string file in files)
            {
                string scriptLine = $"EXECUTE dbo.uspTagFileGetTC '[0]\\{Path.GetFileName(file)}', @ipv_iUpdUserID,@ipv_dcCostPerTag";
                sb.AppendLine(scriptLine);
            }

            string scriptToWrite = script.Replace("{Placeholder}", sb.ToString());

            File.WriteAllText(Path.Combine(filesSourceFolder, scriptFile), scriptToWrite);
        }
    }
}
