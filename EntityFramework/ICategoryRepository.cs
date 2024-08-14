using FashionMart.Models;

namespace FashionMart.EntityFramework
{
    public interface ICategoryRepository
    {
        Task RemoveCategory(int categoryId);
        Task<Category> FetchCategoryById(int id);
        Task InsertCategory(Category category);
        Task<List<Category>> FetchCategoryDetail();
        Task ModifyCategory(Category category);
    }
}
