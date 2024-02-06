using System.ComponentModel.DataAnnotations;

namespace Blog.Dto.CommentDto
{
    public class UpdateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}
