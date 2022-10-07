using Blog.Data;
using Blog.Models;
using Blog.Repository.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogInfoController : Controller
    {
        private readonly IBlogservice blogservice;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment _env;
        public BlogInfoController(IBlogservice blogservice,ICategoryService categoryService, IWebHostEnvironment env)
        {
            this.categoryService = categoryService;
            this.blogservice = blogservice; 
            _env = env; 
        }

        public IActionResult BlogList()
        {
            var obj = blogservice.GetBlogs().Result;
            return View(obj);
        }

        public IActionResult Add()
        {
            ViewBag.Category = new SelectList(categoryService.GetCategories().Result, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public async Task<IActionResult> AddBlog(BlogData obj, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                obj.BlogId = Guid.NewGuid();
                obj.DateCreated = DateTime.UtcNow;
                if (image != null)
                {

                    //Getting the Image upload from server converuing to a url
                    var name = Path.Combine(_env.WebRootPath + "/images",Path.GetFileName(image.FileName));
                     image.CopyTo(new FileStream(name, FileMode.Create));
                    obj.ImageUrl = $"images/{image.FileName}";

                    // Resize(image.FileName);

                }
                _ = await blogservice.AddBlog(obj);

                TempData["AddBlog"] = "Blog saved Successfully";
                return RedirectToAction("BlogList");
            }
            ModelState.AddModelError(string.Empty, "Use correct format");
            return View(obj);
        }

        [ActionName("Delete")]
        public IActionResult Delete(Guid? id)
        {
            if (id != Guid.Empty)
            {
                var checkBlog = blogservice.GetBlogById(id).Result;
                if (checkBlog != null)
                {
                    // first delete the image in the server
                    Action<string> deleteImg = blogservice.DeleteImage;
                    deleteImg(checkBlog.ImageUrl);

                    // then delete the actual blog
                    blogservice.DeleteBlog(checkBlog);

                    TempData["DeleteBlog"] = "Blog deleted Successfully";
                    return RedirectToAction("BlogList");
                }
                TempData["DeleteBlog"] = "Blog cannot be deleted";
                return RedirectToAction("BlogList");

            }
            return NotFound();
        }

        //===== Go to Edit View //
        public IActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();

            }
            var checkBlog = blogservice.GetBlogById(id).Result;
            //var category = await _contect.Category.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
            if (checkBlog != null)
            {
                ViewBag.Category = new SelectList(categoryService.GetCategories().Result, "CategoryId", "CategoryName");
                return View(checkBlog);
            }
            return NotFound();

        }

        // =====  edit the category //
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlog(BlogData obj, IFormFile image, List<string> existingTags)
        {
            //============
            // Check if new tags  are not added
            //                            ==============================

            if (obj.Tags == null)
            {
                obj.Tags = new List<string>();
            }

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //Getting the Image upload from server converting to a url
                    var name = Path.Combine(_env.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    obj.ImageUrl = "images/" + image.FileName;

                }
            
                obj.Tags.AddRange(existingTags);
                var date = obj.DateCreated.ToUniversalTime();
                obj.DateCreated = date;
                await blogservice.UpdateBlog(obj);
                TempData["EditBlog"] = "Blog edited successfully";
                return RedirectToAction("BlogList");
            }
            ModelState.AddModelError(string.Empty, "Invalid format");
            return View("Edit",obj);
        }

    }

}


