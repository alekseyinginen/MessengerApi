using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@".{8,}", ErrorMessage = "Invalid password!")]
        public string Password { get; set; }
    }
}
