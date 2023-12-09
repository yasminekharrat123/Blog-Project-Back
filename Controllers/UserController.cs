using Microsoft.AspNetCore.Mvc;
using Blog.Dto.UserDto;
using Blog.Middleware;
using Blog.Models;
using Blog.ResponseExceptions;
using Blog.Responses;
using Blog.Services;

namespace TP1.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public UserLoginResponse Login([FromBody] LoginDto userLoginDto)
        {
            return _userService.Login(userLoginDto.Email, userLoginDto.Password);
        }

        [HttpPost("signup")]
        public UserSignupResponse Signup([FromBody] RegisterDto createUserDto)
        {
           
            return _userService.Signup(createUserDto);
           
        }

        [HttpGet("whoami")]
        [ServiceFilter(typeof(AuthMiddleware))]
        public UserResponseDto GetUserFromJWT()
        {
            var user = (User)HttpContext.Items["user"]!;
            if (user==null) { throw new BadRequestException("Unexpected?"); }
            return new UserResponseDto(user);
        }
    }
}
