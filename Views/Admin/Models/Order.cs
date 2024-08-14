using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; } // Foreign Key from User Identity
        public DateTime OrderDate { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
