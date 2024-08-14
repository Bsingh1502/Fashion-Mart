using FashionMart.EntityFramework;
using FashionMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing product-related actions in the FashionMart application.
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly IProductRepository prodRepo;
        private readonly ICategoryRepository catRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productRepository">The repository to manage product data.</param>
        /// <param name="categoryRepository">The repository to manage category data.</param>
        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.prodRepo = productRepository;
            this.catRepo = categoryRepository;
        }

        /// <summary>
        /// Displays the list of all products.
        /// </summary>
        /// <returns>The Products view with a list of all products and categories.</returns>
        public async Task<IActionResult> Index()
        {
            var products = await prodRepo.FetchProducts();
            ViewData["Categories"] = await catRepo.FetchCategoryDetail();
            return View("Products", products);
        }

        /// <summary>
        /// Displays the filtered products based on the selected category.
        /// </summary>
        /// <param name="selectedCategory">The ID of the selected category.</param>
        /// <returns>The Products view with the list of filtered products.</returns>
        [HttpGet("[controller]/Filter/{selectedCategory}")]
        public async Task<IActionResult> Filter(int selectedCategory)
        {
            var products = await prodRepo.FetchByCategoryId(selectedCategory);
            ViewData["Categories"] = await catRepo.FetchCategoryDetail();
            return View("Products", products);
        }

        /// <summary>
        /// Searches for products based on the provided filter name.
        /// </summary>
        /// <param name="filterName">The name or keyword to filter products by.</param>
        /// <returns>The Products view with the list of filtered products.</returns>
        [HttpGet("[controller]/Search")]
        public async Task<IActionResult> Filter(string filterName)
        {
            var products = await prodRepo.FetchByProd(filterName);
            ViewData["Categories"] = await catRepo.FetchCategoryDetail();
            return View("Products", products);
        }

        /// <summary>
        /// Filters the products based on the selected category.
        /// </summary>
        /// <param name="selectedCategory">The ID of the selected category.</param>
        /// <returns>A redirection to the Filter action with the selected category.</returns>
        public async Task<IActionResult> FilterData(int selectedCategory)
        {
            return RedirectToAction("Filter", new { selectedCategory = selectedCategory });
        }

      
      
    }
}
