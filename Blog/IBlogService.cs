using Blog.Blog.Models;
using Blog.Dto.BlogDto;
using Blog.Models;
using Blog.Services;
using BlogModel = Blog.Models.Blog; 
namespace Blog.Blog
{
    public interface IBlogService : IGenericService<BlogModel>
    {
        public List<MinimalBlogResponseDto> GetBlogs(PaginationParams pagination = null, BlogFilterParams filter = null);
        public MinimalBlogResponseDto GetBlogById(int blogId);
        public Task<MinimalBlogResponseDto> CreateBlog(CreateBlogDto createDto);
        public Task<MinimalBlogResponseDto> UpdateBlog(UpdateBlogDto updateDto);
        public Task<MinimalBlogResponseDto> DeleteBlog(int blogId);
        public int GetCommentCountByBlog(BlogModel blog);
        public IEnumerable<Comment> GetCommentsByBlog(int page, int limit, BlogModel blog);
        public int GetLikeCountByBlog(BlogModel blog);

    }
}
