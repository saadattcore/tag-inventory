using System.ComponentModel.DataAnnotations;

namespace Transcore.Services.Membership.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password is required")]       
        [StringLength(50, ErrorMessage = "Password must be atleast 12 character long", MinimumLength = 12)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
