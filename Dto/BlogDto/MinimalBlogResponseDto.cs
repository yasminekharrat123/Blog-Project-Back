using Blog.Dto.CommentDto;
using Blog.Models;
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
        public int UserId { get; set; }

        public ICollection<CommentResponseDto> Comments { get; set; } 
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
            UserId = blog.UserId;
            if (blog.Comments != null)
            {
                Comments = blog.Comments.Select(c => new CommentResponseDto(c)).ToList();

            }


        }

    }
}
