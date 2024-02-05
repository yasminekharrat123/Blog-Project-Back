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
        public int CommentCount { get; set; } = 0;
        public int PublisherId { get; set; }


        public MinimalBlogResponseDto()
        {

        }
        public MinimalBlogResponseDto(BlogModel blog)
        {
            BlogId = blog.Id; 
            Title = blog.Title;
            Description = blog.Description;
            MarkdownPath = blog.MarkdownPath;
            PublisherId = blog.UserId; 
            if (blog.Likes != null)
            {
                LikeCount = blog.Likes.Count(); 
            }
            if (blog.Comments!= null)
            {
                CommentCount = blog.Comments.Count();
            }


        }

    }
}
