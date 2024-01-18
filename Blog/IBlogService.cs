using Blog.Blog.Models;
using Blog.Dto.BlogDto;
using Blog.Models;
using BlogModel = Blog.Models.Blog; 
namespace Blog.Blog
{
    public interface IBlogService
    {
        public List<MinimalBlogResponseDto> GetBlogs(PaginationParams pagination = null, BlogFilterParams filter = null);
        public Task<BlogModel> CreateBlog(CreateBlogDto createDto);
        public Task<BlogModel> UpdateBlog(UpdateBlogDto updateDto);
        public BlogModel DeleteBlog(int blogId);
        public int GetCommentCountByBlog(BlogModel blog);
        public IEnumerable<Comment> GetCommentsByBlog(int page, int limit, BlogModel blog);


    }
}
