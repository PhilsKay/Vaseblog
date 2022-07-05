using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogInfoController : Controller
    {
        private readonly Context _contect;
        private readonly IWebHostEnvironment _env;
        public BlogInfoController(Context context, IWebHostEnvironment env)
        {
            _contect = context;
            _env = env; 
        }

        public async Task<IActionResult> BlogList()
        {
            var obj = await _contect.BlogData.Include(c =>c.CategoryName).ToListAsync();
            return View(obj);
        }

        public IActionResult Add()
        {
            ViewBag.Category = new SelectList(_contect.Category.ToList(), "CategoryId", "CategoryName");
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
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    obj.ImageUrl = "images/" + image.FileName;

                }
                await _contect.BlogData.AddAsync(obj);
                await _contect.SaveChangesAsync();
                TempData["AddBlog"] = "Blog saved Successfully";
                return View();
            }
            ModelState.AddModelError(string.Empty, "Use correct format");
            return View(obj);
        }



        [ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                var checkBlog = await _contect.BlogData.FindAsync(id);
                if (checkBlog != null)
                {
                    _contect.BlogData.Remove(checkBlog);
                    _contect.SaveChanges();
                    TempData["DeleteBlog"] = "Blog deleted Successfully";
                    return RedirectToAction("BlogList");
                }
                TempData["DeleteBlog"] = "Blog cannot be deleted";
                return RedirectToAction("BlogList");

            }
            return NotFound();
        }

        //===== Go to Edit View //
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();

            }
            var checkBlog = await _contect.BlogData.FindAsync(id);
            //var category = await _contect.Category.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
            if (checkBlog != null)
            {
                return View(checkBlog);
            }
            return NotFound();

        }

        // =====  edit the category //
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlog(BlogData obj, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //Getting the Image upload from server converuing to a url
                    var name = Path.Combine(_env.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    obj.ImageUrl = "images/" + image.FileName;

                }
                _contect.BlogData.Update(obj);
                await _contect.SaveChangesAsync();
                TempData["EditBlog"] = "Blog edited successfully";
                return RedirectToAction("BlogList");
            }
            ModelState.AddModelError(string.Empty, "Invalid format");
            return View(obj);
        }
    }

}


