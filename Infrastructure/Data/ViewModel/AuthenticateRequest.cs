using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.ViewModel
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}