using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
	public class LoginDTO
	{
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public required string Email { get; set; }

        //[Required(ErrorMessage = "The UserName field is required.")]
        //public required string UserName { get; set; }


        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

