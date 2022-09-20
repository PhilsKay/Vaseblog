using Blog.Models;

namespace Blog.Repository.IServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int? id);
        Task<Category> AddCategory(Category category);
        Task<Category> UpdateCategory(Category category);   
        void DeleteCategory(Category category);
    }
}
