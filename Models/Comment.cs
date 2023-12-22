using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{

    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}