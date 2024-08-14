using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }

        // Navigation property for Category
        public Category Category { get; set; }
        // Navigation property for CartItems
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Navigation property for OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
