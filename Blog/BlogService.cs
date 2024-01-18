using Blog.Context;
using BlogModel = Blog.Models.Blog;
using Microsoft.EntityFrameworkCore;
using Blog.Services;
using Blog.Blog.Models;
using Blog.Dto.BlogDto;
using Blog.Services.FileService;
using Blog.Services.Comments;

using Blog.Models;
using LikeService;

namespace Blog.Blog
{
    public class BlogService : GenericService<BlogModel>, IBlogService
    {
        private readonly IFileService _fileService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public BlogService(BlogDbContext context , FileService fileService , CommentService  commentService , ILikeService likeService ) : base(context)
        {
            _fileService = fileService;
            _commentService = commentService; 
            _likeService = likeService;
        }
        public List<MinimalBlogResponseDto> GetBlogs(PaginationParams pagination = null, BlogFilterParams filter = null)
        {
            var query = _repository.AsQueryable();

            // Apply filters 
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(x => x.Title.Contains(filter.SearchTerm) || x.Description.Contains(filter.SearchTerm));
                }

                if (filter.CreatedBefore.HasValue)
                {
                    query = query.Where(x => x.CreationDate <= filter.CreatedBefore);
                }

                if (filter.CreatedAfter.HasValue)
                {
                    query = query.Where(x => x.CreationDate >= filter.CreatedAfter);
                }

                if (filter.PublisherId.HasValue)
                {
                    query = query.Where(x => x.UserId == filter.PublisherId);
                }

            }

            // Apply pagination if provided
            if (pagination != null)
            {
                query = query
                    .Skip((pagination.PageNumber - 1) * pagination.Limit)
                    .Take(pagination.Limit);
            }

            var data = query
            .Select(blog => new MinimalBlogResponseDto
            {
                BlogId = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                MarkdownPath = blog.MarkdownPath,
                LikeCount = GetLikeCountByBlog(blog),
                CommentCount = GetCommentCountByBlog(blog)
            })
            .ToList();


            return data ; 
        }

        public async Task<BlogModel> UpdateBlog(UpdateBlogDto updateDto)
        {
            var existingBlog =  this.FindById(updateDto.BlogId);
            if (existingBlog != null)
            {
                existingBlog.Title = updateDto.Title ?? existingBlog.Title;
                existingBlog.Description = updateDto.Description ?? existingBlog.Description;
                if (updateDto.UploadedMarkdownFile != null)
                {
                    var prevMarkdownPath = existingBlog.MarkdownPath; 
                    existingBlog.MarkdownPath = await _fileService.UploadMarkdownFile(updateDto.UploadedMarkdownFile);
                    if (existingBlog.MarkdownPath !=null)
                    {
                        if (prevMarkdownPath!= null )
                        {
                            await _fileService.DeleteFile(prevMarkdownPath);
                        }
                    }else
                    {
                        existingBlog.MarkdownPath = prevMarkdownPath; 
                    }

                }

                return  this.Update(existingBlog);
            }


            return null; 
            

        }

        public async Task<BlogModel> CreateBlog(CreateBlogDto createDto)
        {
            var newBlog = new BlogModel();
            newBlog.Title = createDto.Title;
            newBlog.Description = createDto.Description;
            newBlog.UserId = createDto.UserId;

            /// Waiting File service
            newBlog.MarkdownPath = await _fileService.UploadMarkdownFile(createDto.UploadedMarkdownFile);
            newBlog.CreationDate = DateTime.UtcNow;
           return  this.Create(newBlog);

        }

        public BlogModel DeleteBlog(int blogId)
        {
            return this.Delete(blogId); 
        }

        public int GetCommentCountByBlog(BlogModel blog)
        {
            return _commentService.GetCommentCountByBlog(blog); 
        }
        public IEnumerable<Comment> GetCommentsByBlog(int page, int limit, BlogModel blog)
        {
            return _commentService.GetCommentsByBlog(page , limit , blog); 
        }
        public int GetLikeCountByBlog(BlogModel blog)
        {
            return _likeService.GetLikeCountByBlog(blog); 
        }

    }
}
