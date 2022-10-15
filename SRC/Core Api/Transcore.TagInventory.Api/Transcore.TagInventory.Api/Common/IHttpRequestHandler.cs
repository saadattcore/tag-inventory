using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transcore.TagInventory.Web.Common
{
    public interface IHttpRequestHandler
    {
        void SavePostedShipmentFiles(HttpRequest httpRequest,string rootImportFolder ,string uploadFilesBasePath);

        //string ValidateModel(ModelStateDictionary propDictionary);


    }
}
