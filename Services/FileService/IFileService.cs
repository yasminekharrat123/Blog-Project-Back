namespace Blog.Services.FileService
{
    public interface IFileService
    {
        Task<string> UploadMarkdownFile(IFormFile markdownFile);
        Task<IEnumerable<string>> UploadImageFiles(IEnumerable<IFormFile> imageFiles);

        Task<string> UploadImageFile(IFormFile imageFile); 
        Task<bool> DeleteFiles(IEnumerable<string> fileUrls);
        Task<bool> DeleteFile(string fileUrl);
    }
}
