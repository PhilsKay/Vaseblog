using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogRoles
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }    
    }
}
