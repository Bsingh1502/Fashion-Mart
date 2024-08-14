using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

    namespace FashionMart.Models
{
    // created By Bhanu Partap Singh
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation property for related Products
        public virtual ICollection<Product> Products { get; set; }
    }
}
