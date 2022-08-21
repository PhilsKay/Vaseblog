using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> Index(int? page, string title, string tag)
        {
            //Check if the action is not from the search function
            if (title != null)
            {
                var search = await _context.BlogData.Include(m => m.CategoryName)
                    .Where(s => s.Title.ToLower().Contains(title.ToLower())).OrderByDescending(c => c.DateCreated).ToListAsync();
                ViewBag.Search = $"Search result for : {title}";
                foreach (var data in search)
                {
                    ViewBag.DateInAgoFormat = date(data.DateCreated.ToLocalTime().Ticks);//calling the date method and adding to the viewbag
                }

                return View(search.ToPagedList(page ?? 1, 9));
            }
            //Check if the action is not from the Tags function
            if (tag != null)
            {
                var search = await _context.BlogData.Include(m => m.CategoryName)
                    .Where(s => s.Tags.Contains(tag)
                    ).OrderByDescending(c => c.DateCreated).ToListAsync();
                ViewBag.Search = $"Search result for : {tag}";
                foreach (var data in search)
                {
                    ViewBag.DateInAgoFormat = date(data.DateCreated.ToLocalTime().Ticks);//calling the date method and adding to the viewbag
                }

                return View(search.ToPagedList(page ?? 1, 9));
            }
            var blogs = await _context.BlogData.Include(m => m.CategoryName).OrderByDescending(c => c.DateCreated).ToListAsync();
            foreach(var data in blogs)
            {
                ViewBag.DateInAgoFormat = date(data.DateCreated.ToLocalTime().Ticks);//calling the date method and adding to the viewbag
            }

            return View(blogs.ToPagedList(page??1,9));
        }

        //For displaying the blog categories for filtering
        public async Task<IActionResult> BlogFilter()
        {
            return PartialView("_BlogFilterPartial",await _context.Category.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //This method gets the blog's date in ago format 
        public string date(Int64 blogDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - blogDate);
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

