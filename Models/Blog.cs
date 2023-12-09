namespace Blog.Models
{
    
    public class Blog
{
    public string Title { get; set; }
    public string Body { get; set; }

    public Blog(string Title, string Body)
    {
        this.Title = Title;
        this.Body = Body;
    }
}}