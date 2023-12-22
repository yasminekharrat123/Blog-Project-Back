using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public string MarkdownPath { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}