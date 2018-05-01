using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@".{8,}", ErrorMessage = "Invalid password!")]
        public string Password { get; set; }

        public string PictureURL { get; set; }
    }
}
