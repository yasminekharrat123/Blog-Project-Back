namespace Blog.Blog.Models
{
    public class BlogFilterParams
    {
        public string ? SearchTerm { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public int? PublisherId { get; set; }
    }
}
