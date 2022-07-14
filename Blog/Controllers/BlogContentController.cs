using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class BlogContentController : Controller
    {
        private readonly ILogger<BlogContentController> _logger;
        private readonly Context _context;

        public BlogContentController(ILogger<BlogContentController> logger,Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> data(Guid? id)
        {
            // confirm if the id is not empty
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            //check if the id is in the database
            var check = await _context.BlogData.Where(c => c.BlogId == id).FirstOrDefaultAsync();
            if(check != null)
            {
                var getCategoryName = await _context.Category.Where(c => c.CategoryId == check.CategoryId).FirstOrDefaultAsync();                //show the contents in view
                ViewBag.Category = getCategoryName.CategoryName;
                return View(check);
            }
            return NotFound();
        }
    }
}
