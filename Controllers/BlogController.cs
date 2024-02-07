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
using LikeService;

namespace Blog.Controllers
{
    [Route("blog")]

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ILikeService _likeService;
        public BlogController(IBlogService blogService, ILikeService likeService)
        {
            this._blogService = blogService;
            this._likeService = likeService;
        }


        [HttpPost]
        [ServiceFilter(typeof(AuthMiddleware))]
        public async Task<IActionResult> Create([FromForm] CreateBlogRequestDto createBlogRequestDto)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }

            CreateBlogDto createBlogDto;
            try
            {
                createBlogDto = new CreateBlogDto { Title = createBlogRequestDto.Title, Description = createBlogRequestDto.Description, UploadedMarkdownFile = createBlogRequestDto.UploadedMarkdownFile, UserId = user.Id };
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Missing or invalid data");
            }

            MinimalBlogResponseDto blogResponse = await _blogService.CreateBlog(createBlogDto);
            if (blogResponse == null)
            {
                throw new Exception("Couldn't create blog");
            }

            return Ok(blogResponse);
        }


        [HttpGet("{blogId}")]
        public MinimalBlogResponseDto GetBlogById([FromRoute] int blogId)
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



            List<MinimalBlogResponseDto> blogs = _blogService.GetBlogs(pagination, filter);


            return blogs;
        }


        [HttpDelete("{blogId}")]
        [ServiceFilter(typeof(AuthMiddleware))]
        [ServiceFilter(typeof(BlogOwnerMiddleware))]

        public async Task<IActionResult> DeleteBlog([FromRoute] int blogId)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }



            MinimalBlogResponseDto blog = await _blogService.DeleteBlog(blogId);


            return Ok(blog);
        }
        [HttpPatch("{blogId}")]
        [ServiceFilter(typeof(AuthMiddleware))]
        [ServiceFilter(typeof(BlogOwnerMiddleware))]
        public async Task<IActionResult> UpdateBlog([FromRoute] int blogId, [FromForm] UpdateBlogDto updateBlogDto)
        {
            updateBlogDto.BlogId = blogId;
            BlogModel blog = _blogService.FindById(updateBlogDto.BlogId);
            if (blog == null)
            {
                throw new NotFoundException("No Blog with such Id");
            }


            MinimalBlogResponseDto updatedBlog = await _blogService.UpdateBlog(updateBlogDto);


            return Ok(updatedBlog);
        }

        [HttpPost("{blogId}/like")]
        [ServiceFilter(typeof(AuthMiddleware))]
        public IActionResult Like([FromRoute] int blogId)
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user == null) { throw new BadRequestException("Unexpected?"); }
            if (_likeService.isLiked(blogId, user))
            {
                _likeService.Dislike(new BlogModel { Id = blogId }, user);
                return Ok(new { Message = "Blog Disliked", BlogId = blogId, UserId = user.Id });
            }
            else
            {
                _likeService.Like(new BlogModel { Id = blogId }, user);
                return Ok(new { Message = "Blog Liked", BlogId = blogId, UserId = user.Id });
            }



        }
        [HttpGet("{blogId}/like")]

        public IActionResult getBlogLikes([FromRoute] int blogId, [FromQuery] int page=-1, [FromQuery] int limit=-1)
        {
          
            IEnumerable<Like> likes = _likeService.GetLikesByBlog(new BlogModel { Id = blogId }, page, limit);
            ICollection<int> userIds = likes.Select(l => l.UserId).ToList();
            return Ok(new { blogId = blogId, likesUserIds = userIds   });

        }
    }
}
