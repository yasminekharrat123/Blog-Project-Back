using Blog.Dto.UserDto;
using Blog.Models;

namespace Blog.Responses
{
    public class UserSignupResponse
    {
        public string AccessToken { get; set; }
        public UserResponseDto user { get; set; }
        public UserSignupResponse(string accessToken, User _user) : base()
        {
            AccessToken = accessToken;
            user = new UserResponseDto(_user);
        }
    }
}
