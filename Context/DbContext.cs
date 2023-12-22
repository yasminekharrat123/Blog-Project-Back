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
        public DbSet<Blog.Models.Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Like>  Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog.Models.Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId);


            modelBuilder.Entity<Comment>()
        .HasOne(c => c.ParentComment)
        .WithMany(c => c.Replies)
        .HasForeignKey(c => c.ParentCommentId)
        .IsRequired(false);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Blog)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.BlogId);

            modelBuilder.Entity<Like>()
       .HasOne(l => l.User)
       .WithMany(u => u.Likes)
       .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Blog)
                .WithMany(b => b.Likes)
                .HasForeignKey(l => l.BlogId);
            modelBuilder.Entity<Like>()
        .HasKey(l => new { l.UserId, l.BlogId });

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Blog)
                .WithMany(b => b.Reports)
                .HasForeignKey(r => r.BlogId);
        }

        }
    }
