using FashionMart.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.EntityFramework
{
    /// <summary>
    /// Repository for managing product data
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Constructor to inject the DbContext dependency
        /// </summary>
        public ProductRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves products by their category ID from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Product>> FetchByCategoryId(int id)
        {
            if (id == 0)
            {
                return await _context.Products.Include(p => p.Category).ToListAsync();
            }
            return await _context.Products.Include(p => p.Category).Where(p => p.CategoryId == id).ToListAsync();
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <returns></returns>
        public async Task InsertProducts(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database
        /// </summary>
        /// <returns></returns>
        public async Task ModifyProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all products from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> FetchProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        /// <summary>
        /// Deletes a product by its ID from the database
        /// </summary>
        /// <returns></returns>
        public async Task RemoveProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves all products that match a given name from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> FetchByProd(string name)
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return products.Where(res => res.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        /// <summary>
        /// Retrieves a product by its ID from the database
        /// </summary>
        /// <returns></returns>
        public async Task<Product> FetchById(int id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }
     
    }
}
