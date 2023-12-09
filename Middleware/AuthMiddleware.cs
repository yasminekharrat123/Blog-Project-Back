using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Blog.Context;
using Blog.ResponseExceptions;
using Blog.Services;

namespace TP1.Middleware
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthMiddleware : ActionFilterAttribute
    {
        private readonly IUserService _userService;

        public AuthMiddleware(IUserService userService)
        {
            _userService = userService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
           
            var tokenFromHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(tokenFromHeader) && tokenFromHeader.StartsWith("Bearer "))
            {
                var token = tokenFromHeader.Substring("Bearer ".Length).Trim();
                var user = _userService.GetUserByJwt(token);
                if (user != null) {
                    context.HttpContext.Items["user"] = user;
                    base.OnActionExecuting(context);
                    return;
                }
            }
            throw new UnauthorizedException("Invalid token");
            
        }
    }
}
