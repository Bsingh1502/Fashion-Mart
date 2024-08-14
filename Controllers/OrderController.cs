using FashionMart.EntityFramework;
using FashionMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing user orders in the FashionMart application.
    /// </summary>
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderRepository">The repository to manage order data.</param>
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Displays the list of orders for the current user.
        /// </summary>
        /// <returns>The Order view with the list of user's orders.</returns>
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _orderRepository.FetchOrders(userId);

            return View("Order", order);
        }
    }
}
