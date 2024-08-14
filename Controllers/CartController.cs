using FashionMart.EntityFramework;
using FashionMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing the shopping cart operations.
    /// </summary>
    public class CartController : Controller
    {
        private readonly ICartRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class with a specified cart repository.
        /// </summary>
        /// <param name="cartRepository">The cart repository to interact with the data layer.</param>
        public CartController(ICartRepository cartRepository)
        {
            this.repository = cartRepository;
        }

        /// <summary>
        /// Displays the cart for the current user.
        /// </summary>
        /// <returns>An IActionResult that renders the cart view.</returns>
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await repository.FetchCartUsingId(userId);
            return View("Cart", cart);
        }
      
        /// <summary>
        /// Deletes a specific item from the user's cart.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>A redirect to the cart Index action to refresh the view.</returns>
        public async Task<IActionResult> DeleteItem(int cartItemId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await repository.DeleteCart(cartItemId);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates the quantity of a specific product in the user's cart.
        /// </summary>
        /// <param name="ProductId">The product ID for which the quantity needs to be updated.</param>
        /// <param name="change">The operation to perform ("increase" or "decrease").</param>
        /// <param name="quantity">The current quantity of the product in the cart.</param>
        /// <returns>A redirect to the cart Index action to refresh the view.</returns>
        public async Task<IActionResult> UpdateQuantity(int ProductId, string change, int quantity)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (change == "increase")
            {
                await repository.ModifyCartDetails(userId, ProductId, 1);
            }
            else if (change == "decrease" && quantity > 0)
            {
                await repository.ModifyCartDetails(userId, ProductId, -1);
            }

            return RedirectToAction("Index"); // Redirect to the cart view to show updated quantities
        }

    }
}
