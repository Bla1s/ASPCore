using System.ComponentModel.DataAnnotations;

namespace ASPCore.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public bool Remember { get; set; }
    }
}
