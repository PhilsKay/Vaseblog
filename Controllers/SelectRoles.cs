using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class SelectRoles : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager; 
        private readonly Context _context;

        public SelectRoles(RoleManager<IdentityRole> roleManager, 
            Context context, UserManager<IdentityUser> userManager )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create(BlogRoles role)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await roleManager.RoleExistsAsync(role.name);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role.name));
                    if (result.Succeeded)
                    {
                        ViewData["roleCreate"] = "Role Created Successfully";
                        return View(role);
                    }
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View();

                    }

                }
                ModelState.AddModelError(string.Empty, "Role already exist");
                return View();

            }
            ModelState.AddModelError(string.Empty, "Input the correct format");
            return View(role);  
           
        }

        public IActionResult UserRole()
        {
            return View();
        }

        [HttpPost]
        [ActionName("UserRole")]
        public async Task<IActionResult> UserRole(string user)
        {
            //Find the user from the database
            var find = await _context.Users.Where(c => c.UserName.Equals(user)).FirstOrDefaultAsync();
            //Check if the result is null or exist
            if(find != null)
            {
                //find if the user is already present in the role specified
                var userRoleExist = await userManager.IsInRoleAsync(find, "Admin");
                if (!userRoleExist)
                {
                    // If the user not present then add to role
                    var result = await userManager.AddToRoleAsync(find, "Admin");
                    if (result.Succeeded)
                    {
                        ViewData["roleUserCreate"] = "User added as Admin";
                        return View();
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View();
                    }
                }
                
                ViewData["UserExist"] = "User already exist in the role";
                return View();

            }
            ViewData["notExist"] = "User does not exist";
            return View();
        }
    }
}
