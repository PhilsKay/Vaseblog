using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public IdentityUser Author { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
