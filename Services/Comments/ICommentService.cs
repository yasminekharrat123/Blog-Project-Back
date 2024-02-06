using Blog.Models;

namespace Blog.Services.Comments
{
    public interface ICommentService
    {
        int GetCommentCountByBlog(Models.Blog blog);
        IEnumerable<Comment> GetCommentsByBlog(int page, int limit, Models.Blog blog);
        int GetRepliesCountByComment(int commentId);
        public IEnumerable<Comment> GetRepliesByComment(int commentId, int recursionDepth, int page, int limit);
        Comment CreateComment(int userId, int blogId, int? parentCommentId, string content);
        Comment UpdateComment(int commentId, string updatedContent);
        Comment FindComment(int commentId);
        void DeleteComment(int commentId);
    }
}
