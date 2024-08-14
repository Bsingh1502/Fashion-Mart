using FashionMart.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.EntityFramework
{
    /// <summary>
    /// Repository for managing order-related operations in the FashionMart application.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to be used for order operations.</param>
        public OrderRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        /// <returns>A list of all orders, including their details and associated products.</returns>
        public async Task<List<Order>> FetchOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all orders associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are to be retrieved.</param>
        /// <returns>A list of orders for the specified user, including their details and associated products.</returns>
        public async Task<List<Order>> FetchOrders(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID, including its details and associated products.</returns>
        public async Task<Order> FetchOrderId(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">The order to add.</param>
        public async Task InsertOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="order">The order to update.</param>
        public async Task ModifyOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an order from the database based on its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        public async Task RemoveOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
