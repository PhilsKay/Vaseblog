using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Areas.Admin.Controllers
{
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
    }
}
