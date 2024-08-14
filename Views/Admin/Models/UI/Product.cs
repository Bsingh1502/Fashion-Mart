using System.ComponentModel.DataAnnotations;

namespace FashionMart.Models.UI
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Image is Required")]
        public IFormFile ImageUrl { get; set; }

        public List<Models.Category> Categories { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is Required")]
        public string NewCategory { get; set; }

        public List<Models.Product> Products { get; set; }

        public Product()
        {
            Categories = new List<Models.Category>();
            Products = new List<Models.Product>();
        }

    }
}
