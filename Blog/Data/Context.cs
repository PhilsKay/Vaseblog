using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class Context : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)   
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<BlogData> BlogData { get; set; }
        public DbSet<MainComment> MainComment { get; set; }
        public DbSet<SubComment> subComments { get; set; }




    }
}
