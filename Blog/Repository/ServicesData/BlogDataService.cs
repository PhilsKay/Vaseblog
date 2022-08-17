using Blog.Data;
using Blog.Models;
using Blog.Repository.IServices;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;




namespace Blog.Repository.ServicesData
{
    public class BlogDataService : ICategoryService, IBlogservice
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> userManager;
        //private readonly IHttpContextAccessor httpContextAccessor;

        public BlogDataService(Context context, UserManager<IdentityUser> user/* IHttpContextAccessor httpContextAccessor*/)
        {
            _context = context;
            this.userManager = user;
           // this.httpContextAccessor = httpContextAccessor;
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

        public async void AddSubComment(SubComment subComment)
        {
            await _context.subComments.AddAsync(subComment);
            await _context.SaveChangesAsync();
        }

        public async Task<BlogData> Comment(CommentViewModel comment)
        {
            var blog = await GetBlogById(comment.BlogId);
            var httpContext = new HttpContextAccessor().HttpContext;
            if (comment.MainCommentId == 0)
            {
                blog.Comments = blog.Comments ?? new List<MainComment>();
                blog.Comments.Add(new MainComment
                {
                    Body = comment.Body,
                    Author = httpContext.User.Identity.Name,
                    DateCreated = DateTime.UtcNow,
                    SubComments = new List<SubComment>()
                }) ;
                await UpdateBlog(blog);
            }
            else
            {
                var subComment = new SubComment()
                {
                    MainCommentId = comment.MainCommentId,  
                    Body = comment.Body,
                    Author = httpContext.User.Identity.Name,
                    DateCreated = DateTime.UtcNow
                };
                AddSubComment(subComment);
            }
            return blog;
        }

        //public async Task<MainComment> AddComment(MainComment comment)
        //{
        //    await _context.MainComment.AddAsync(comment);
        //    await _context.SaveChangesAsync();
        //    return comment;
        //}
        //public async Task<ActionResult<MainComment>> CreateComment(BlogData post, MainComment comment, ClaimsPrincipal claimsPrincipal)
        //{
        //    if (post.BlogId == Guid.Empty)
        //        return new BadRequestResult();
        //    var blog = await GetBlogById(post.BlogId);
        //    if (blog == null)
        //        return new NotFoundResult();
        //    var body = comment.Body;
        //    comment.Author = await _userManager.GetUserAsync(claimsPrincipal);
        //    comment.Blog = blog;
        //    comment.DateCreated = DateTime.UtcNow;
        //    if()

        //    return comment;
        //}

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
            var blog = await _context.BlogData.Include(c => c.CategoryName)
                .Include(c => c.Comments).ThenInclude(c => c.SubComments)
                .Where(c => c.BlogId == id).FirstOrDefaultAsync();
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

        //public async Task<MainComment> GetMainCommentId(int? id)
        //{
        //    var MainComment = await _context.MainComment.Include(m => m.SubComments).Where(
        //        c => c.Id == id).FirstOrDefaultAsync();
        //    return MainComment;
        //}


        public async Task UpdateBlog(BlogData blogData)
        {
            _context.BlogData.Update(blogData);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
