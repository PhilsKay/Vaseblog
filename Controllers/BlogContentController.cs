﻿using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Blog.Repository.IServices;
using Blog.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Blog.Models;

namespace Blog.Controllers
{
    /// <summary>
    /// delegate that points to all the date converter 
    /// that shows when a blog is posted
    /// </summary>
    /// <param name="date"></param>
    /// <returns> string</returns>
    public delegate string DateCreatedDelegate(Int64 date);
    public class BlogContentController : Controller
    {
        private readonly ILogger<BlogContentController> _logger;
        private readonly IBlogservice blogservice;
        private readonly ICategoryService categoryService;

        public BlogContentController(ILogger<BlogContentController> logger,IBlogservice blogservice, ICategoryService categoryService)
        {
            _logger = logger;
            this.blogservice = blogservice;
            this.categoryService = categoryService;
        }

        public IActionResult data(Guid? id)
        {
            // confirm if the id is empty
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            //check if the id is in the database
            var check = blogservice.GetBlogById(id).Result;
            if(check != null)
            {
                //Find the category Id of the blog since the category name is null in the check variable
                var getCategoryName = categoryService.GetCategoryById(check.CategoryId).Result;
                //show the contents of the category id in the viewbag and get the category name to display in the page
                ViewData["Category"] = $"{getCategoryName.CategoryName}";
                //cull the delegate variable
                DateCreatedDelegate dateDelegate = date;
                ViewData["dateInAgo"] = $"{dateDelegate(check.DateCreated.ToLocalTime().Ticks)}";//calling the date method and adds to the viewbag
                return View(check);
            }
            return NotFound();
        }


        public IActionResult LatestBlog(int? page)
        {
            var latest = blogservice.GetBlogs().Result.Take(3).ToList();

            return PartialView("_LatestBlogsPartial", latest);
        }

        public IActionResult ArchiveBlog()
        {
            var archive = blogservice.GetArchiveBlogs().Result;

            return PartialView("_ArchiveBlogPartial", archive);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddComment(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
                return View("data",comment.BlogId);
            var result = blogservice.Comment(comment).Result;
            //stops the comment model from showing in the view
            ViewData.ModelState.Clear();
                return View("data",result);
        }

        [Authorize]
        public async Task<IActionResult> LikePost(Guid Id)
        {
            if(Id == Guid.Empty)
            {
                return NotFound();
            }    
             var blog = await blogservice.SaveLike(Id);
            var user = new HttpContextAccessor().HttpContext;

            TempData["LikePost"] = $"Thanks {user.User.Identity.Name},your review is well appreciated";
            return View("data",blog);
        }


        //This method gets the blog's date in ago format 
        public string date(Int64 blogDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.ToLocalTime().Ticks - blogDate);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }


    }
}