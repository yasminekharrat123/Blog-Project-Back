using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}