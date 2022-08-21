using Blog.Models;
using Blog.ViewModels;
using System.Security.Claims;

namespace Blog.Repository.IServices
{
    public interface IBlogservice
    {
        Task<List<BlogData>> GetBlogs();
        Task<List<BlogData>> GetArchiveBlogs();
        Task<BlogData> GetBlogById(Guid? id);
        Task<BlogData> Comment(CommentViewModel comment);
        Task AddSubComment(SubComment subComment);
        Task<BlogData> AddBlog(BlogData blogData);  
        Task UpdateBlog(BlogData blogData);   
        void DeleteBlog(BlogData blogData);   
    }
}
