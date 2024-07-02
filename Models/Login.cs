using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    // created By Bhanu Partap Singh
    public class Login
    {
        // The Email property is required and must be a valid email address
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // The Password property is required 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
