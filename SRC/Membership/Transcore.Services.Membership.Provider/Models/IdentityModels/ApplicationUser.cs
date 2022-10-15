using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.Services.Membership.IdentityModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        private DateTime _createdDateTime;

        public ApplicationUser()
        {
            _createdDateTime = DateTime.Now;
        }

        
        [NotMapped]        
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "DateTime")]

        public DateTime CreatedDateTime
        {
            get
            {

                return this._createdDateTime;

            }
            set { this._createdDateTime = DateTime.Now; }
        }

        [Column(TypeName = "DateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
