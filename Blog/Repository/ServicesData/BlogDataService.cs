using Blog.Data;
using Blog.Models;
using Blog.Repository.IServices;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository.ServicesData
{
    public class BlogDataService : ICategoryService, IBlogservice
    {
        private readonly Context _context;
        public BlogDataService(Context context)
        {
            _context = context;
        }
        
        public async Task<BlogData> AddBlog(BlogData blogData)
        {
            await _context.BlogData.AddAsync(blogData);
            await _context.SaveChangesAsync();
            return blogData;
        }

        public async Task<Category> AddCategory(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public void DeleteBlog(BlogData blogData)
        {
            _context.BlogData.Remove(blogData);
            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _context.Category.Remove(category);
            _context.SaveChanges();
        }

        public async Task<List<BlogData>> GetArchiveBlogs()
        {
            var archive = await _context.BlogData.Include(m => m.CategoryName).OrderBy(c => c.DateCreated).ToListAsync();
            return archive;

        }

        public async Task<BlogData> GetBlogById(Guid? id)
        {
            var blog = await _context.BlogData.Where(c => c.BlogId == id).FirstOrDefaultAsync();
            return blog;
        }

        public async Task<List<BlogData>> GetBlogs()
        {
            var latest = await _context.BlogData.Include(m => m.CategoryName).OrderByDescending(c => c.DateCreated).ToListAsync();

            return latest;

        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            var category = await _context.Category.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<BlogData> UpdateBlog(BlogData blogData)
        {
            _context.BlogData.Update(blogData);
            await _context.SaveChangesAsync();
            return blogData;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
