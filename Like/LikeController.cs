using Blog.Models;
using LikeService;
using Microsoft.AspNetCore.Mvc;

[Route("like")]
public class LikeController : Controller
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("like")]
    public void Like(Blog.Models.Blog blog, User user)
    {
        _likeService.Like(blog,user);
    }

    [HttpPost("dislike")]
    public void Dislike(Blog.Models.Blog blog, User user)
    {
        _likeService.Dislike(blog,user);
    }

    [HttpGet("count/{blogId}")]
    public int GetLikeCountByBlog(Blog.Models.Blog blog)
    {
        return _likeService.GetLikeCountByBlog(blog);
    }

    [HttpGet("getLikesByBlog/{blogId}")]
    public IEnumerable<Like> GetLikesByBlog(Blog.Models.Blog blog, int page)
    {
        return _likeService.GetLikesByBlog(blog, page);
    }
}   