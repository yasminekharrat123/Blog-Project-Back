using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Blog.Models;
using Blog.ResponseExceptions;
using Blog.Services;
using System.Linq;
using BlogModel = Blog.Models.Blog;


namespace Blog.Middleware
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class BlogOwnerMiddleware : ActionFilterAttribute
    {
        // TO be replaced with
        // private readonly IBlogService _blogService;

        private readonly IGenericService<BlogModel> _GenericService;


        //param to be replaced with IBlogService blogService
        public BlogOwnerMiddleware(IGenericService<BlogModel> blogService)

        {
            // _blogService = blogService;
            _GenericService = blogService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = (User)context.HttpContext.Items["user"];
            if (user == null)
            {
                throw new UnauthorizedException("Invalid User");
            }

            if (!context.ActionArguments.TryGetValue("blogId", out var blogIdObj) || !(blogIdObj is int blogId))
            {
                //blogId is not in the route or is not an int
                throw new UnauthorizedException("Blog not found");
            }

            // var blog = _blogService.GetBlogById(blogId);
            var blog = _GenericService.FindById(blogId);
            if (blog == null)
            {
                throw new UnauthorizedException("Blog not found");
            }

            if (blog.UserId != user.Id)
            {
                throw new UnauthorizedException("User is not the owner of the blog");
            }

            base.OnActionExecuting(context);
        }
    }
}
