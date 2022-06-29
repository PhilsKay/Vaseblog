using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class BlogData
    {
        [Key]
        public Guid BlogId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Should not be more than 20 characters")]
        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }

        [Required]
        [ForeignKey("CategoryId")]
        [Display(Name ="Category")]
        [Column("Category Name")]
        public virtual Category BlogCategory { get; set; }

        [Required]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Content")]
        [MaxLength(int.MaxValue)]
        public string Content { get; set; }
    }
}
