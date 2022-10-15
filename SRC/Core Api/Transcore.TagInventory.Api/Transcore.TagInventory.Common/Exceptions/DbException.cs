using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Transcore.TagInventory.Common.Exceptions
{
    public class DataBaseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
