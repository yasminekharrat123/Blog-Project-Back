using Blog.Models;

namespace Blog.Services.Comments
{
    public interface ICommentService
    {
        int GetCommentCountByBlog(Models.Blog blog);
        IEnumerable<Comment> GetCommentsByBlog(int page, int limit, Models.Blog blog);
        int GetRepliesCountByComment(int commentId);
        public IEnumerable<Comment> GetRepliesByComment(int commentId, int recursionDepth, int page, int limit);
        Comment CreateComment(User user, Models.Blog blog, string content);
        Comment UpdateComment(int commentId, string updatedContent);
        void DeleteComment(int commentId);
        Comment CreateReply(int commentId, User user, string content);
    }
}
