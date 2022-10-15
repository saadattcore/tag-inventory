using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Common.Exceptions
{
    public class FileSpaceMissingException : Exception
    {
        public string ErrorMessage { get; set; }
        public FileSpaceMissingException(string message)
        {
            ErrorMessage = message;
        }
    }
}
