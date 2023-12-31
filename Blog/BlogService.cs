using Blog.Context;
using BlogModel = Blog.Models.Blog;
using Microsoft.EntityFrameworkCore;
using Blog.Services;
using Blog.Blog.Models;
using Blog.Dto.BlogDto;
using Blog.Services.FileService;
using Blog.Models;

namespace Blog.Blog
{
    public class BlogService : GenericService<BlogModel>, IBlogService
    {
        private readonly FileService _fileService;
        public BlogService(BlogDbContext context , FileService fileService) : base(context)
        {
            _fileService = fileService; 
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
                MarkdownPath = blog.MarkdownPath  ,
                LikeCount = blog.Likes.Count(),
                CommentCount = blog.Comments.Count()
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
                    existingBlog.MarkdownPath = await _fileService.UploadMarkdownFile(updateDto.UploadedMarkdownFile);
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
    }
}
