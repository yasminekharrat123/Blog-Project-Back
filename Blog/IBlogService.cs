using Blog.Blog.Models;
using Blog.Dto.BlogDto;
using BlogModel = Blog.Models.Blog; 
namespace Blog.Blog
{
    public interface IBlogService
    {
        public List<MinimalBlogResponseDto> GetBlogs(PaginationParams pagination = null, BlogFilterParams filter = null);
        public Task<BlogModel> CreateBlog(CreateBlogDto createDto);
        public Task<BlogModel> UpdateBlog(UpdateBlogDto updateDto);
        public BlogModel DeleteBlog(int blogId); 
    }
}
