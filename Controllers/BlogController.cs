using Blog.Blog;
using Blog.Dto.BlogDto;
using Blog.Middleware;
using Blog.Models;
using Blog.ResponseExceptions;
using BlogModel = Blog.Models.Blog;

using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Blog.ReponseExceptions;
using Blog.Blog.Models;

namespace Blog.Controllers
{
    [Route("blog")]

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService; 
        public BlogController(IBlogService blogService)
        {
            this._blogService = blogService;
        }


        [HttpPost]
        [ServiceFilter(typeof(AuthMiddleware))]
        public  async Task<IActionResult> Create([FromForm] CreateBlogRequestDto createBlogRequestDto)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }

            CreateBlogDto createBlogDto; 
            try
            {
                 createBlogDto = new CreateBlogDto { Title = createBlogRequestDto.Title, Description = createBlogRequestDto.Description, UploadedMarkdownFile = createBlogRequestDto.UploadedMarkdownFile, UserId = user.Id };
            }catch (Exception ex)
            {
            throw new    BadRequestException("Missing or invalid data");
            }

            MinimalBlogResponseDto blogResponse =  await _blogService.CreateBlog(createBlogDto);
            if (blogResponse == null)
            {
                throw new Exception("Couldn't create blog"); 
            }

            return Ok(blogResponse); 
        }


        [HttpGet("{blogId}")]
        public  MinimalBlogResponseDto GetBlogById([FromRoute] int blogId)
        {



            MinimalBlogResponseDto blog = _blogService.GetBlogById(blogId); 
            

            return blog;
        }


        [HttpGet]
        //[ServiceFilter(typeof(AuthMiddleware))]
        public List<MinimalBlogResponseDto> GetBlogs([FromQuery] PaginationParams pagination, [FromQuery] BlogFilterParams filter)
        {
            //  var user = (User)HttpContext.Items["user"]!;
            // if (user == null) { throw new BadRequestException("Unexpected?"); }



            List<MinimalBlogResponseDto> blogs = _blogService.GetBlogs(pagination,filter);


            return blogs;
        }


        [HttpDelete("{blogId}")]
        [ServiceFilter(typeof(AuthMiddleware))]
        public async Task<IActionResult> DeleteBlog([FromRoute] int blogId)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }



            MinimalBlogResponseDto blog = await _blogService.DeleteBlog(blogId);


            return Ok(blog);
        }
        [HttpPatch("{blogId}")]
        [ServiceFilter(typeof(AuthMiddleware))]
        public async Task<IActionResult> UpdateBlog([FromRoute] int blogId , [FromForm] UpdateBlogDto updateBlogDto)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }
            updateBlogDto.BlogId = blogId;
            BlogModel blog = _blogService.FindById(updateBlogDto.BlogId); 


            if(blog == null)
            {
                throw new NotFoundException("No Blog with such Id"); 
            }
            if(blog.UserId != user.Id)
            {
                throw new UnauthorizedException("You are not the owner of this blog");
            }


            MinimalBlogResponseDto updatedBlog = await _blogService.UpdateBlog(updateBlogDto);


            return Ok(updatedBlog);
        }
    }
}
