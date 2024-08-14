using System.ComponentModel.DataAnnotations;

namespace FashionMart.Models
{
    // Done by Gurwinder Singh
    public class Register
    {
        // The first name of the user.
        public string FirstName { get; set; }

        // The last name of the user.
        public string LastName { get; set; }

        // The password of the user is required
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // The password confirmation field
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // The email of the user is required with email validation
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

    }
}

