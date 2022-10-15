using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Transcore.TagInventory.Web.Common
{
    public interface IHttpRequestHandler
    {
        Dictionary<string, string> RetrievePostedData(out long shipmentID, out short boxType);

        string ValidateModel(ModelStateDictionary propDictionary);


    }
}
