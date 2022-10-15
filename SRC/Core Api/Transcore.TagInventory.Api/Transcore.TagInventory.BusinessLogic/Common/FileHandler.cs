using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<ShipmentFile> ParseUploadedFiles(string filesSourceFolder, long shipmentID, byte boxType)
        {
            var importedFiles = new List<ShipmentFile>();

            // string dirPath = Path.Combine(uploadedFilesPath, shipmentID.ToString()) + Path.DirectorySeparatorChar.ToString();

            //Directory.GetDirectories(uploadedFilesPath).ToList().Select(f => !f.ToLower().Contains("export"));

            var filePaths = Directory.GetFiles(filesSourceFolder, "*.txt", SearchOption.AllDirectories);

            foreach (var fileLocation in filePaths)
            {
                var fileName = Path.GetFileName(fileLocation);

                var fileContent = File.ReadAllText(fileLocation);

                string fileContentOrignal = fileContent;

                fileContent = fileContent.Replace("\r", string.Empty);

                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileContent)) continue;

                var lines = fileContent.Split('\n').ToList();

                if (lines.Count == 0) continue;

                var fileHeader = lines[0] + "\t" + lines[1].Split('\t')[0] + "\t" + "Filename " + fileName;

                ShipmentFile importedFile = fileHeader.ToImportFile();

                importedFile.Header = lines[0];

                importedFile.BoxTypeID = boxType;

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

                        string boxHeader = $"{page[0]}\n{page[1]}";

                        ReceivedBox receivedBox = page.ToReceivedBox();

                        //receivedBox.Section = string.Join(Environment.NewLine, page);

                        receivedBox.Header = boxHeader;

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
