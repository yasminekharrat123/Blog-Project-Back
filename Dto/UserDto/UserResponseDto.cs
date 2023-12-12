using Blog.Models;

namespace Blog.Dto.UserDto
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public UserResponseDto(User user)
        {
            Username = user.Username;
            Email = user.Email;
            Role = user.Role;
        }
    }
}
