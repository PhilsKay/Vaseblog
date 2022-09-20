using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class PostLike
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }
        [ForeignKey("BlogId")]
        public BlogData PostId { get; set; }
        public string User { get; set; }

        public bool UserLike { get; set; }
        public DateTime DateLiked { get; set; }
    }
}
