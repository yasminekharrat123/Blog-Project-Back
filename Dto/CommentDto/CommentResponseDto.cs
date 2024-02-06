using Blog.Models;

namespace Blog.Dto.CommentDto
{
    public class CommentResponseDto

    {
        public int Id { get; set; }

        public string Content { get; set; }
        public int UserId { get; set; }
        public int? BlogId { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime Date { get; set; }

        public CommentResponseDto()
        {

        }
       public  CommentResponseDto(Comment c)
        {
            Content = c.Content; 
            Id = c.Id;
            UserId = c.UserId;
            BlogId = c.BlogId;
            ParentCommentId = c.ParentCommentId;
            Date = c.Date; 

        }
    }
}
