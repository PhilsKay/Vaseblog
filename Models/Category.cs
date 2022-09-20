using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category")]
        [Column("Category Name")]
        [StringLength(20,ErrorMessage ="Should not be more than 20 characters")]
        public string CategoryName { get; set; }
    }
}
