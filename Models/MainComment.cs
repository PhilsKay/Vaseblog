namespace Blog.Models
{
    public class MainComment : Comment
    {
        public List<SubComment> SubComments { get; set; }
    }
}
