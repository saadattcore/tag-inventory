using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.Common.Enums
{
    public enum Lookup
    {
        TagStatus,
        ReceivedBoxStatus,
        IssuedBoxStatus,
        ReceivedBoxType,
        VisualCheckStatus,
        RFIDCheckStatus,
        KitVisualCheckStatus,
        KitRFIDCheckStatus,
        ALL
    }

    public enum ExportPackageType
    {
        CostTags,
        NoCostTags,
        PartialTags
    }

    public enum TagStatus
    {
        Imported,
        ScannedOK,
        Defective,
        Discarded,
        MissingTag,
        MissingKit,
        Kitted
    }
}
