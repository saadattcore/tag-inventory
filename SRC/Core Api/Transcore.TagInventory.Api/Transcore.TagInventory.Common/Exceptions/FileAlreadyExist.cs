using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Common.Exceptions
{
    public class FileAlreadyExist : Exception
    {
        public FileAlreadyExist(string errorMessage) : base(errorMessage)
        {
            
        }     

    }
}
