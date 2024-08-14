using FashionMart.EntityFramework;
using FashionMart.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using System.Security.Claims;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing checkout processes in the FashionMart application.
    /// </summary>
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController"/> class with specified order and cart repositories.
        /// </summary>
        /// <param name="orderRepository">The repository to manage order data.</param>
        /// <param name="cartRepository">The repository to manage cart data.</param>
        public CheckoutController(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Displays the checkout page.
        /// </summary>
        /// <returns>The Checkout view.</returns>
        public IActionResult Index()
        {
            return View("Checkout");
        }

        /// <summary>
        /// Processes the checkout information and completes the order.
        /// </summary>
        /// <param name="model">The checkout data model containing user input.</param>
        /// <returns>A view showing either a successful checkout or the form to correct input.</returns>
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(Checkout model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = await cartRepository.FetchCartUsingId(userId);
                List<OrderDetail> orderDetails = new List<OrderDetail>();

                // Create order details from cart items
                foreach (var item in cart.CartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };
                    orderDetails.Add(orderDetail);
                }

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    UserId = userId,
                    OrderDetails = orderDetails
                };

                // Save the new order and clear the cart
                await orderRepository.InsertOrder(order);
                await cartRepository.CLearCart(userId);

                ViewData["OrderSuccess"] = true; // Inform the user of a successful order
                return View("Checkout", model);
            }

            // If the model state is not valid, redisplay the form with the current model to allow for corrections.
            return View("Checkout", model);
        }
    }
}
