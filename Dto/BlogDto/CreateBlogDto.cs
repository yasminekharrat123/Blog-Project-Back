namespace Blog.Dto.BlogDto
{
    public class CreateBlogDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile UploadedMarkdownFile { get; set; }
    }
}
