using BlogModel = Blog.Models.Blog;

namespace Blog.Dto.BlogDto
{
    public class MinimalBlogResponseDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = null;
        public string MarkdownPath { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        
    }
}
