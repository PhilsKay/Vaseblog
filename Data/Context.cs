using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)   
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<BlogData> BlogData { get; set; }
        public DbSet<MainComment> MainComment { get; set; }
        public DbSet<SubComment> subComments { get; set; }
        public DbSet<PostLike> PostLike { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BlogData>().OwnsOne(x => x.Tags);
        //    base.OnModelCreating(modelBuilder);

        //}

    }
}
