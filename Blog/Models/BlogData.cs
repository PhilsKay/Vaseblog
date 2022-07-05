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
        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category CategoryName { get; set; }

        [Display(Name = "ImageUrl")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Content")]
        [MaxLength(int.MaxValue)]
        public string Content { get; set; }

        [Display(Name = "DateTime Created")]
        public DateTime DateCreated { get; set; }
    }

}
