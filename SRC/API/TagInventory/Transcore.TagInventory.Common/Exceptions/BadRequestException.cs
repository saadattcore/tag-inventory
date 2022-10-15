using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string error) : base(error)
        {

        }
    }
}
