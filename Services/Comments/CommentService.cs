using Blog.Models;
using Blog.Context;
using Blog.ResponseExceptions;
using System;
using MySqlX.XDevAPI.CRUD;
using System.ComponentModel.Design;

namespace Blog.Services.Comments
{
    public class CommentService:ICommentService
    {
        private readonly BlogDbContext _context;

        public CommentService(BlogDbContext context)
        {
            _context = context;
        }



        public Comment FindComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                throw new ResponseExceptions.BaseResponseException($"Comment with ID {commentId} does not exist.", ResponseExceptions.StatusCodes.NOT_FOUND);
            }
            return comment;
        }


        private void LoadReplies(Comment comment, int depth)
        {
            if (depth <= 0) return;

            // Eagerly load the replies of the current comment
            _context.Entry(comment)
                    .Collection(c => c.Replies)
                    .Load();

            // Recursively load the replies of each reply
            foreach (var reply in comment.Replies)
            {
                LoadReplies(reply, depth - 1);
            }
        }


        public int GetCommentCountByBlog(Models.Blog blog)
        {
            return _context.Comments.Count(c => c.Id == blog.Id);
        }


        public int GetRepliesCountByComment(int commentId)
        {
            Comment comment = FindComment(commentId);
            return _context.Comments.Count(c => c.ParentCommentId == commentId);
        }

        
        public IEnumerable<Comment> GetCommentsByBlog(int page, int limit, Models.Blog blog)
        {
            return _context.Comments
                            .Where(c => c.BlogId == blog.Id)
                            .OrderBy(c => c.Date) 
                            .Skip((page - 1) * limit)
                            .Take(limit)
                            .ToList();
        }




        public IEnumerable<Comment> GetRepliesByComment(int commentId, int recursionDepth , int page , int limit)
        {
            var comment = FindComment(commentId);

            // Apply pagination to the first level of replies
            var paginatedReplies = _context.Comments
                                           .Where(c => c.ParentCommentId == commentId)
                                           .Skip((page - 1) * limit)
                                           .Take(limit)
                                           .ToList();

            // Load replies recursively for each comment in the paginated set
            foreach (var reply in paginatedReplies)
            {
                LoadReplies(reply, recursionDepth - 1);
            }

            return paginatedReplies;
        }

        public Comment CreateComment(int userId, int? blogId, int? parentCommentId, string content)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == userId);
            Comment? parentComment = null;
            if(parentCommentId != null)
            {
                parentComment = _context.Comments.FirstOrDefault(c => c.Id == parentCommentId);
                if (parentComment == null)
                {
                    throw new ResponseExceptions.BaseResponseException($"Comment with ID {parentCommentId} does not exist.", ResponseExceptions.StatusCodes.NOT_FOUND);
                }
            }
            Models.Blog? blog = null;
            if(blogId != null) { 
                blog = _context.Blogs.FirstOrDefault(c => c.Id == blogId);
                if(blog == null)
                {
                    throw new ResponseExceptions.BaseResponseException($"Blog with ID {blogId} does not exist.", ResponseExceptions.StatusCodes.NOT_FOUND);
                }
            }

            var comment = new Comment { User = user, Blog = blog, ParentComment = parentComment ,   Content = content };
            
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment UpdateComment(int commentId, string updatedContent)
        {
            var comment = FindComment(commentId);
            comment.Content = updatedContent;
            _context.SaveChanges();
            return comment;
        }

        public void DeleteComment(int commentId)
        {
            var comment = FindComment(commentId);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            
        }

        public Comment CreateReply(int commentId, User user, string content)
        {
            var parentComment = FindComment(commentId);
            var reply = new Comment { User = user, ParentComment = parentComment, Content = content };
            _context.Comments.Add(reply);
            _context.SaveChanges();
            return reply;
        }
    
}
}
