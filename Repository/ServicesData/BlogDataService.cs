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
    /// <summary>
    /// repository for blog and category
    /// </summary>
    public class BlogDataService : ICategoryService, IBlogservice
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment _env;


        public BlogDataService(Context context, UserManager<IdentityUser> user, IWebHostEnvironment env)
        {
            _context = context;
            this.userManager = user;
            _env = env;
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

        public async Task AddSubComment(SubComment subComment)
        {
            await _context.subComments.AddAsync(subComment);
            await _context.SaveChangesAsync();
        }

        public async Task<BlogData> Comment(CommentViewModel comment)
        {
            var blog = await GetBlogById(comment.BlogId);
            var httpContext = new HttpContextAccessor().HttpContext;
            if (comment.MainCommentId > 0)
            {
                var subComment = new SubComment()
                {
                    MainCommentId = comment.MainCommentId,
                    Body = comment.Body,
                    Author = httpContext.User.Identity.Name,
                    DateCreated = DateTime.UtcNow
                };
                await AddSubComment(subComment);
            }
            else
            {
                blog.Comments = blog.Comments ?? new List<MainComment>();
                blog.Comments.Add(new MainComment
                {
                    Body = comment.Body,
                    Author = httpContext.User.Identity.Name,
                    DateCreated = DateTime.UtcNow,
                    SubComments = new List<SubComment>()
                });
                await UpdateBlog(blog);
            }
            return blog;
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

        public void DeleteImage(string url)
        {
            string img = Path.Combine(_env.WebRootPath,url);
            FileInfo filename = new FileInfo(img);
            if (filename.Exists)
            {
                System.IO.File.Delete(filename.FullName);
                filename.Delete();
            }
        }

        public async Task<List<BlogData>> GetArchiveBlogs()
        {
            var archive = await _context.BlogData.Include(m => m.CategoryName).OrderBy(c => c.DateCreated).Take(3).ToListAsync();
            return archive;

        }


        public async Task<BlogData> GetBlogById(Guid? id)
        {
            var blog = await _context.BlogData.Include(c => c.CategoryName)
                .Include(c => c.Comments).ThenInclude(c => c.SubComments).Include(c => c.Likes)
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

        public int GetTotalBlogs()
        {
            Func<Task<List<BlogData>>> countBlogs = GetBlogs;
            return countBlogs.Invoke().Result.Count();
        }

        public int GetTotalUsers()
        {
            return _context.Users.Count();
        }

      


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
        /// <summary>
        /// method responsible fot the blog Like funtionality
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public async Task<BlogData> SaveLike(Guid postid)
        {
            //get the current user
            var httpcontext = new HttpContextAccessor().HttpContext;
            var user = httpcontext.User.Identity.Name;

            var item = await GetBlogById(postid);

            item.Likes = item.Likes ?? new List<PostLike>();
            item.Comments = item.Comments ?? new List<MainComment>();
            var dupe = item.Likes.FirstOrDefault(e => e.User == user);
            //checks if the user already like/unlike the post
            if (dupe == null)
            {
                // if null adds the new user like to the post
                _context.PostLike.Add(new PostLike()
                {
                    Id = Guid.NewGuid(),
                    BlogId = postid,
                    User = user,
                    UserLike = true,
                    DateLiked = DateTime.UtcNow,
                });

            }
            else
            {
                //if not null then it updats the the user like in post.
                switch (dupe.UserLike)
                {
                    case true:
                        dupe.UserLike = !dupe.UserLike;
                        break;
                        default:
                        dupe.UserLike = true;  
                        break;
                }
            }
            _context.SaveChanges();
            // reload the post again is because we may have an additional "like" since this process was executed.
            var post = await GetBlogById(postid);
            return post;

        }

        public int GetTotalLikes()
        {
            return _context.PostLike.Count();
        }
    }
}

