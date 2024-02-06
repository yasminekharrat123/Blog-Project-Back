using System.ComponentModel.DataAnnotations;

namespace Blog.Dto.CommentDto
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public int BlogId { get; set; }
    }
}
