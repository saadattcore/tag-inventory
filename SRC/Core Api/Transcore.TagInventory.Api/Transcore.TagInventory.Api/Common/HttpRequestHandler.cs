using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Transcore.TagInventory.Common.Exceptions;

namespace Transcore.TagInventory.Web.Common
{
    public class HttpRequestHandler : IHttpRequestHandler
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _appSettings;

        public HttpRequestHandler(IWebHostEnvironment hosting, IConfiguration appSettings)
        {
            this._hostingEnvironment = hosting;
            _appSettings = appSettings;
        }


        /// <summary>
        /// Take posted files from form and return their content and name.
        /// </summary>
        /// <returns></returns>
        /// 

        public void SavePostedShipmentFiles(HttpRequest httpRequest, string rootFilesUploadFolder, string shipmentFolder)
        {
            for (int i = 0; i < httpRequest.Form.Files.Count; i++)
            {
                IFormFile httpPostedFile = httpRequest.Form.Files[i];

                if (httpPostedFile != null)
                {
                    byte[] fileBuffer = new byte[httpPostedFile.Length];

                    var fs = httpPostedFile.OpenReadStream();

                    fs.Read(fileBuffer, 0, fileBuffer.Length);                                       

                    if (!Directory.Exists(rootFilesUploadFolder))
                    {
                        Directory.CreateDirectory(rootFilesUploadFolder);
                    }

                    if (Directory.GetFiles(Path.Combine(rootFilesUploadFolder), "*.txt", SearchOption.AllDirectories).ToList().Select(filePath => Path.GetFileName(filePath)).Contains(httpPostedFile.FileName))
                    {
                        throw new FileAlreadyExist($"{httpPostedFile.FileName} already uploaded");
                    }

                    if (!Directory.Exists(shipmentFolder))
                    {
                        Directory.CreateDirectory(shipmentFolder);
                    }

                    string fileToSave = Path.Combine(shipmentFolder, httpPostedFile.FileName);

                    File.WriteAllBytes(fileToSave, fileBuffer);

                    fs.Dispose();

                }
            }
        }

        /// <summary>
        /// Save files to disk
        /// </summary>
        /// <param name="filesDict"></param>
        public void SaveFiles(Dictionary<string, string> filesDict, HttpRequest httpRequest)
        {
            var fileuploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, "/UploadedFiles");
        }

        //public string ValidateModel(ModelStateDictionary modelStateDictionary)
        //{

        //    StringBuilder sb = new StringBuilder();

        //    foreach (ModelState modelState in modelStateDictionary.Values)
        //    {
        //        foreach (ModelError modelError in modelState.Errors)
        //        {
        //            sb.AppendLine(modelError.ErrorMessage);
        //        }
        //    }
        //    var errors = sb.ToString();

        //    return errors;
        //}
    }
}