using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Common.Exceptions;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Core;

namespace Inventory.Services.Common
{
    public static class FileHandler
    {
        public static List<TagShipmentHeader> ParseImportFiles(Dictionary<string, string> files, long shipmentID, short boxType)
        {
            var importedFiles = new List<TagShipmentHeader>();

            foreach (var file in files)
            {
                var fileName = file.Key;

                var fileContent = file.Value;

                fileContent = fileContent.Replace("\r", string.Empty);

                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileContent)) continue;

                var lines = fileContent.Split('\n').ToList();

                if (lines.Count == 0) continue;

                var fileHeader = lines[0] + "\t" + lines[1].Split('\t')[0] + "\t" + "Filename " + fileName;

                var importedFile = fileHeader.ToImportFile();

                importedFile.ReceivedBoxes = new List<Transcore.TagInventory.Entity.Core.ReceivedBox>();

                Dictionary<int, List<string>> pages = new Dictionary<int, List<string>>();

                int pageCounter = 0;

                try
                {
                    do
                    {


                        int indexOfEmptyLine = lines.IndexOf("");

                        if (indexOfEmptyLine == -1)
                        {
                            StringBuilder sb = new StringBuilder();

                            sb.AppendLine("Following error(s) occured when importing the file. ");
                            sb.AppendLine("");
                            sb.AppendLine($"There is formating error in file {fileName}. Spacing is missing");
                            sb.AppendLine("");
                            sb.Append(" ");
                            sb.AppendLine("Please insert empty line and import again");  
                            
                            throw new FileSpaceMissingException(sb.ToString());                           
                        }
                        var page = lines.GetRange(1, indexOfEmptyLine - 1);

                        var receivedBox = page.ToReceivedBox();

                        receivedBox.ShipmentID = shipmentID;

                        receivedBox.BoxTypeID = boxType;

                        importedFile.ReceivedBoxes.Add(receivedBox);

                        pages.Add(++pageCounter, page);

                        lines.RemoveRange(0, ++indexOfEmptyLine);


                    } while (lines.IndexOf("") != -1);

                    importedFile.ShipmentID = shipmentID;

                    importedFiles.Add(importedFile);
                }
                catch (Exception)
                {
                    StringBuilder sb = new StringBuilder();
                   
                    sb.AppendLine("Following error(s) occured when importing the file. ");
                    sb.AppendLine("");
                    sb.AppendLine($"There is formating error in file {fileName}. Spacing is missing");
                    sb.AppendLine("");
                    sb.Append(" ");
                    sb.AppendLine("Please insert empty line and import again");
                   
                    throw new FileSpaceMissingException(sb.ToString());
                }

            }

            return importedFiles;
        }
    }
}
