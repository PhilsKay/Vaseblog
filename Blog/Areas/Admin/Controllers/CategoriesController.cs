using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly Context _contect;
        public CategoriesController(Context context)
        {
            _contect = context;
        }
        public async Task<IActionResult> CategoryList()
        {
            var obj = await _contect.Category.ToListAsync();
            return View(obj);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public async Task<IActionResult> AddCategory(Category obj)
        {
            if (ModelState.IsValid)
            {
                await _contect.Category.AddAsync(obj);
                await _contect.SaveChangesAsync();
                TempData["AddCategory"] = "Category saved Successfully";
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError(string.Empty, "Use correct format");
            return View("Add",obj);
        }



        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var category = await _contect.Category.FindAsync(id);
                if (category != null)
                {
                    _contect.Category.Remove(category);
                    _contect.SaveChanges();
                    TempData["DeleteCategory"] = "Category deleted Successfully";
                    return RedirectToAction("CategoryList");
                }
                TempData["DeleteCategory"] = "Category cannot be deleted";
                return RedirectToAction("CategoryList");

            }
            return NotFound();
        }

        //===== Go to Edit View //
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var category = await _contect.Category.FindAsync(id);   
            //var category = await _contect.Category.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
            if (category != null)
            {
                return View(category);
            }
            return NotFound();

        }

        // =====  edit the category //
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category obj)
        {
                if (ModelState.IsValid)
                {
                        _contect.Category.Update(obj);
                        await _contect.SaveChangesAsync();
                        TempData["EditCategory"] = "Category edited successfully";
                        return RedirectToAction("CategoryList");
                }
                ModelState.AddModelError(string.Empty, "Invalid format");
                return View(obj);
        }
    }

}

