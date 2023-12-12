using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Blog.Models;
using Blog.ResponseExceptions;
using Blog.Services;

namespace Blog.Middleware
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminMiddleware : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = (User) context.HttpContext.Items["user"]!;
            if( user == null)
            {
                throw new UnauthorizedException("Invalid User");
            } if (user.Role != UserRole.Admin)
            {
                throw new UnauthorizedException("User must be an admin");
            }
        }
    }
}
