using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace Blog.Models
{

    public class Like
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }
        public int BlogId { get; set; }
        [JsonIgnore]
        public Blog Blog { get; set; }

    }
}