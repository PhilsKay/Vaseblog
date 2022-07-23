using Blog.Models;

namespace Blog.Repository.IServices
{
    public interface IBlogservice
    {
        Task<List<BlogData>> GetBlogs();
        Task<List<BlogData>> GetArchiveBlogs();
        Task<BlogData> GetBlogById(Guid? id);    
        Task<BlogData> AddBlog(BlogData blogData);  
        Task<BlogData> UpdateBlog(BlogData blogData);   
        void DeleteBlog(BlogData blogData);   
    }
}
