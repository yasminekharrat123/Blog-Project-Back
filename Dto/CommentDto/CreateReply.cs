using System.ComponentModel.DataAnnotations;

namespace Blog.Dto.CommentDto
{
    public class CreateReplyDto
    {
        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ParentCommentId { get; set; }
    }
}
