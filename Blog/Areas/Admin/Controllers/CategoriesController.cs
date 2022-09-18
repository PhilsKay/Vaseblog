using Blog.Data;
using Blog.Models;
using Blog.Repository.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoriesController : Controller
    {
        //configure services in controller
        private readonly ICategoryService categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult CategoryList()
        {
            var obj = categoryService.GetCategories().Result;
            return View(obj);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public IActionResult AddCategory(Category obj)
        {
            if (ModelState.IsValid)
            {
                _ = categoryService.AddCategory(obj).Result;
                TempData["AddCategory"] = $"{obj.CategoryName} has been added to list Successfully";
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError(string.Empty, "Use correct format");
            return View("Add",obj);
        }



        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var category = categoryService.GetCategoryById(id).Result;
                if (category != null)
                {
                    try
                    {
                        categoryService.DeleteCategory(category);
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                    {
                        TempData["ErrorCategory"] = $"{ex.Message}";
                        return RedirectToAction("CategoryList");
                    }
                    TempData["DeleteCategory"] = $"{category.CategoryName} has been deleted from list Successfully";
                    return RedirectToAction("CategoryList");
                }
                TempData["DeleteCategory"] = "Category cannot be deleted";
                return RedirectToAction("CategoryList");

            }
            return NotFound();
        }

        //===== Go to Edit View //
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var category = categoryService.GetCategoryById(id).Result;   
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
        public IActionResult EditCategory(Category obj)
        {
                if (ModelState.IsValid)
                {
                       _ = categoryService.UpdateCategory(obj).Result; 
                        TempData["EditCategory"] = $"Category edited successfully as {obj.CategoryName}";
                        return RedirectToAction("CategoryList");
                }
                ModelState.AddModelError(string.Empty, "Invalid format");
                return View(obj);
        }
    }

}

