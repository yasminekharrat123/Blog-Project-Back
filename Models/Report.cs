using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace Blog.Models
{
    public class Report : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }
        public int BlogId { get; set; }
        [JsonIgnore]
        public Blog Blog { get; set; }

    }
}