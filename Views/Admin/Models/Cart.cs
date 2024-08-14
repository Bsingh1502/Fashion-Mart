using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string UserId { get; set; } // FK for Identity User
        public virtual ICollection<CartItem> CartItems { get; set; }
        public User User { get; set; }
    }
}
