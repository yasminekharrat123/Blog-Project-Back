using Blog.Context;
using Blog.Models;
using Blog.ResponseExceptions;
using Blog.Services;


namespace LikeService
{
    public interface ILikeService
    {
        public void Like(Blog.Models.Blog blog, User user);
        public Boolean isLiked(int blogId, User user); 
        public void Dislike(Blog.Models.Blog blog, User user);
        public Like GetLikeById(int id);
        public int GetLikeCountByBlog(Blog.Models.Blog blog);
        public IEnumerable<Like> GetLikesByBlog(Blog.Models.Blog blog, int page = -1 ,  int limit=-1);

    }

    public class LikeService: GenericService<Like>, ILikeService
    {
        public LikeService(BlogDbContext context) : base(context) 
        {}

        public void Like(Blog.Models.Blog blog, User user)
        {
            //test if user already liked the blog
            var like = _repository.FirstOrDefault(l => l.BlogId == blog.Id && l.UserId == user.Id);
            if (like != null)
                throw new BadRequestException("User already liked this blog");
            var newLike = new Like()
            {
                BlogId = blog.Id,
                UserId = user.Id,
                Date = DateTime.Now
            };
            _repository.Add(newLike);
            _context.SaveChanges();

        }

        public Boolean isLiked(int blogId , User user)
        {
            var like = _repository.FirstOrDefault(l => (l.BlogId == blogId && l.UserId == user.Id));
            if (like == null) return false;
            return true; 

        }
        public void Dislike(Blog.Models.Blog blog, User user)
        {
            var like = _repository.FirstOrDefault(l => l.BlogId == blog.Id && l.UserId == user.Id);               
            if (like == null)
                throw new BadRequestException("User didn't like this blog");
            _repository.Remove(like);
            _context.SaveChanges();
        }

        public Like GetLikeById(int id)
        {
            return _repository.FirstOrDefault(l => l.Id == id);
        }

        public int GetLikeCountByBlog(Blog.Models.Blog blog)
        {
            return _repository.Count(l => l.BlogId == blog.Id);
        }

        //using pagination
        //limit likes per page
        public IEnumerable<Like> GetLikesByBlog(Blog.Models.Blog blog , int page = -1 , int limit = -1)
        {
            var query = _repository.AsQueryable();
            query = query.Where(l => l.BlogId == blog.Id); 
            if (page!=-1 && limit != -1)
            {
                query = query.Skip((page - 1) * limit)
                .Take(limit);
            }

            return query.ToList(); 
        }
    }   
    
}