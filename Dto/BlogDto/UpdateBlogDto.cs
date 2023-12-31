namespace Blog.Dto.BlogDto
{
    public class UpdateBlogDto
    {
      
        public int BlogId { get; set; }
        public string ?Title { get; set; }
        public string? Description { get; set; }
        public IFormFile ?UploadedMarkdownFile { get; set; }
    }
}
