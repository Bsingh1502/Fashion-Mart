using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; } // FK to Cart
        public int ProductId { get; set; } // FK to Product
        public int Quantity { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
