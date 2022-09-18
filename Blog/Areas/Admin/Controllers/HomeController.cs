using Blog.Repository.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogservice _blogService; 

        public HomeController(ILogger<HomeController> logger,IBlogservice blogservice)
        {
            _logger = logger;
            _blogService = blogservice;
        }
        public IActionResult Index()
        {
            ViewData["TotalBlogs"] = $"{_blogService.GetTotalBlogs()}";
            ViewData["TotalUsers"] = $"{_blogService.GetTotalUsers()}";
            ViewData["Likes"] = $"{_blogService.GetTotalLikes()}";
            return View();
        }
    }
}
