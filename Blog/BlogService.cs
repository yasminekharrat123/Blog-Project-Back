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
using Blog.ReponseExceptions;
using static Dapper.SqlMapper;

namespace Blog.Blog
{
    public class BlogService : GenericService<BlogModel>, IBlogService
    {
        private readonly IFileService _fileService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public BlogService(BlogDbContext context , IFileService fileService , ICommentService  commentService , ILikeService likeService ) : base(context)
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
            .ToList();

            List<MinimalBlogResponseDto> minimalData = data.Select(blog =>
            {
                blog.Comments = GetCommentsByBlog(-1, -1, blog).ToList()
                     ; 

                
                var minBlog = new MinimalBlogResponseDto(blog);
                minBlog.CommentCount = GetCommentCountByBlog(blog);
                minBlog.LikeCount = GetLikeCountByBlog(blog);
                
                return minBlog;

            }).ToList(); 

            return minimalData ; 
        }

        public async Task<MinimalBlogResponseDto> UpdateBlog(UpdateBlogDto updateDto)
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
                BlogModel blog = this.Update(existingBlog); 
                return  new MinimalBlogResponseDto(blog);
            }


            return null; 
            

        }

        public async Task<MinimalBlogResponseDto> CreateBlog(CreateBlogDto createDto)
        {
            var newBlog = new BlogModel();
            newBlog.Title = createDto.Title;
            newBlog.Description = createDto.Description;
            newBlog.UserId = createDto.UserId;

            /// Waiting File service
            newBlog.MarkdownPath = await _fileService.UploadMarkdownFile(createDto.UploadedMarkdownFile);
            newBlog.CreationDate = DateTime.UtcNow;
            return new MinimalBlogResponseDto(Create(newBlog));

        }

        public async Task<MinimalBlogResponseDto> DeleteBlog(int blogId)
        {

            BlogModel blog = this.FindById(blogId);
            if (blog == null)
            {
                throw new NotFoundException("Blog Not Found");
            }else
            {

            }
            await _fileService.DeleteFile(blog.MarkdownPath);
            return new MinimalBlogResponseDto(this.Delete(blogId)); 
        }
        public MinimalBlogResponseDto GetBlogById(int blogId)
        {
            var blog = FindById(blogId);
            blog.Comments = GetCommentsByBlog(-1, -1, blog).ToList(); 
            var minBlog =  new MinimalBlogResponseDto(blog); 
            minBlog.CommentCount = GetCommentCountByBlog(blog);
            return minBlog; 
        }

        public int GetCommentCountByBlog(BlogModel blog)
        {
            return _commentService.GetCommentCountByBlog(blog); 
        }
        public IEnumerable<Comment> GetCommentsByBlog(int page , int limit , BlogModel blog)
        {
            return _commentService.GetCommentsByBlog(page , limit , blog); 
        }
        public int GetLikeCountByBlog(BlogModel blog)
        {
            return _likeService.GetLikeCountByBlog(blog); 
        }

    }
}
