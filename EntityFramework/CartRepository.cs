using FashionMart.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.EntityFramework
{
    /// <summary>
    /// Repository for managing cart-related operations in the FashionMart application.
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to be used for cart operations.</param>
        public CartRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the cart associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is to be retrieved.</param>
        /// <returns>The cart associated with the specified user ID, including its items.</returns>
        public async Task<Cart> FetchCartUsingId(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// Adds a product to the user's cart or updates the quantity if the product already exists in the cart.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="productId">The ID of the product to add or update.</param>
        /// <param name="quantity">The quantity of the product to add or update.</param>
        public async Task ModifyCartDetails(string userId, int productId, int quantity)
        {
            var cart = await FetchCartUsingId(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var item = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                var data = new List<CartItem>
                {
                    new CartItem { ProductId = productId, Quantity = quantity }
                };

                if (cart.CartItems == null)
                {
                    cart.CartItems = data;
                }
                else
                {
                    cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes an item from the cart based on the cart item ID.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        public async Task DeleteCart(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart items are to be cleared.</param>
        public async Task CLearCart(string userId)
        {
            var cart = await _context.Carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null && cart.CartItems != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
        }
    }
}
