using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transcore.Services.Membership.Models
{
    public class UserModel
    {
        [JsonProperty("userName")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }


        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password must be atleast 12 character long", MinimumLength = 12)]
        public string Password { get; set; }


        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
