using FashionMart.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing product details and cart operations.
    /// </summary>
    public class DetailController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailController"/> class with specified product and cart repositories.
        /// </summary>
        /// <param name="productRepository">The repository to manage product data.</param>
        /// <param name="cartRepository">The repository to manage cart data.</param>
        public DetailController(IProductRepository productRepository, ICartRepository cartRepository)
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Displays the detail view for a specific product.
        /// </summary>
        /// <param name="productId">The ID of the product to display.</param>
        /// <returns>The product detail view.</returns>
        public async Task<IActionResult> Index(int productId)
        {
            var product = await productRepository.FetchById(productId);
            return View("Detail", product);
        }

        /// <summary>
        /// Adds a specified quantity of a product to the user's cart and redirects to the cart view.
        /// </summary>
        /// <param name="productId">The product ID to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <returns>A redirection to the cart index view.</returns>
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            await cartRepository.ModifyCartDetails(userId, productId, quantity);
            // Logic to add the product to the cart
            // You might need to fetch the product details again or directly add them

            // Redirect to a confirmation page or back to the product list
            return RedirectToAction("Index", "Cart");
        }
    }
}
