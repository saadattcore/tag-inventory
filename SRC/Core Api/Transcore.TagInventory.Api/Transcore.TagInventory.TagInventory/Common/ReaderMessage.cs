using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Entity.Common
{
    class ReaderMessage
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }

    }

    public enum MessageType
    {
        TagNumber,
        CommunicationState
    }
}
