using System.ComponentModel.DataAnnotations;

namespace Jaahub.Models
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "User";
    }


    public class LoginModel
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }



}
