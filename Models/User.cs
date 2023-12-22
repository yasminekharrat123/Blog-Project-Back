using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LastLoginDate { get; set; }

        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public enum UserRole
    {
        SuperAdmin,
        Admin,
        User
    }
}