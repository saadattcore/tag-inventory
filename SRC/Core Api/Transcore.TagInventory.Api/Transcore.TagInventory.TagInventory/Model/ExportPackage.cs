using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Transcore.TagInventory.Entity.Model
{
    public class ExportPackage
    {
        [Required(ErrorMessage ="Please sepecify shipment id")]
        [Range(1,long.MaxValue,ErrorMessage ="Shipment Id cannot be negetive")]
        public long ShipmentID { get; set; } 

        [Required(ErrorMessage ="Please specify if shipment contains free tags or not")]
        public bool ContainsFreeTags { get; set; }
    }
}
