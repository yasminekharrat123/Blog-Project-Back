namespace Blog.Dto.BlogDto
{
    public class CreateBlogRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile UploadedMarkdownFile { get; set; }
    }
}
