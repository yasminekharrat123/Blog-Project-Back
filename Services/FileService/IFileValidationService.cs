namespace Blog.Services.FileService
{
    public interface IFileValidationService
    {
        bool ValidateMarkdownFile(IFormFile file);
        bool ValidateImageFile(IFormFile file);
    }
}
