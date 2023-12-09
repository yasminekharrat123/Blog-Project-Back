using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Context
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
