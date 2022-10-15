using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Transcore.TagInventory.Web.Common
{
    public class HttpRequestHandler : IHttpRequestHandler
    {
        /// <summary>
        /// Take posted files from form and return their content and name.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> RetrievePostedData(out long shipmentID, out short boxType)
        {
            var filesDictionary = new Dictionary<string, string>();

            var httpContext = HttpContext.Current;

            shipmentID = long.Parse(httpContext.Request["shipmentID"]);
            boxType = short.Parse(httpContext.Request["shipmentType"]);


            for (int i = 0; i < httpContext.Request.Files.Count; i++)
            {
                HttpPostedFile httpPostedFile = httpContext.Request.Files[i];

                if (httpPostedFile != null)
                {
                    byte[] fileBuffer = new byte[httpPostedFile.ContentLength];

                    httpPostedFile.InputStream.Read(fileBuffer, 0, httpPostedFile.ContentLength);

                    string fileContent = Encoding.UTF8.GetString(fileBuffer);

                    //fileContent = fileContent.TrimEnd(new char[] { '\t' });

                    filesDictionary.Add(httpPostedFile.FileName, fileContent);

                    //var fileSavePath = Path.Combine(fileuploadPath, httpPostedFile.FileName);
                    //httpPostedFile.SaveAs(fileSavePath);
                }
            }

            return filesDictionary;
        }

        /// <summary>
        /// Save files to disk
        /// </summary>
        /// <param name="filesDict"></param>
        public static void SaveFiles(Dictionary<string, string> filesDict)
        {
            var fileuploadPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
        }

        public string ValidateModel(ModelStateDictionary modelStateDictionary)
        {

            StringBuilder sb = new StringBuilder();

            foreach (ModelState modelState in modelStateDictionary.Values)
            {
                foreach (ModelError modelError in modelState.Errors)
                {
                    sb.AppendLine(modelError.ErrorMessage);
                }
            }
            var errors = sb.ToString();

            return errors;
        }
    }
}