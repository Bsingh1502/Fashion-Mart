using FashionMart.Models;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.EntityFramework
{
    /// <summary>
    /// Repository for managing category data
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Constructor to inject the DbContext dependency
        /// </summary>
        public CategoryRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new category to the database
        /// </summary>
        /// <returns></returns>
        public async Task InsertCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing category in the database
        /// </summary>
        /// <returns></returns>
        public async Task ModifyCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a category by its ID from the database
        /// </summary>
        /// <returns></returns>
        public async Task RemoveCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves all categories from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> FetchCategoryDetail()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Retrieves a category by its ID from the database
        /// </summary>
        /// <returns></returns>
        public async Task<Category> FetchCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

    }
}
