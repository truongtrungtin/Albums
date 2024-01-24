using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
	public class RegisterDTO
	{
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public required string Email { get; set; }

        //[Required(ErrorMessage = "The UserName field is required.")]
        //public required string UserName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Display Name is required.")]
        public required string DisplayName { get; set; }

    }
}

