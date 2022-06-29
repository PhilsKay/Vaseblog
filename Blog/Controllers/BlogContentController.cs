using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class BlogContentController : Controller
    {
        private readonly ILogger<BlogContentController> _logger;

        public BlogContentController(ILogger<BlogContentController> logger)
        {
            _logger = logger;
        }

        public IActionResult data()
        {
            return View();
        }
    }
}
