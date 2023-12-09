using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class User: BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        /*// Navigation properties
        public Admin Admin { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }*/
    }

    public enum UserRole
    {
        SuperAdmin,
        Admin,
        User
    }
}