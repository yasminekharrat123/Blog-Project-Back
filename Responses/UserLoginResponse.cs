using Blog.Dto.UserDto;
using Blog.Models;

namespace Blog.Responses
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public UserResponseDto user { get; set; }

        public UserLoginResponse(string accessToken, User _user) : base()
        {
            AccessToken = accessToken;
            user = new UserResponseDto(_user);
        }
    }
}
