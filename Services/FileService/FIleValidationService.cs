namespace Blog.Services.FileService
{
    public class FileValidationService : IFileValidationService
    {
        private IConfiguration _configuration;

        public FileValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool ValidateImageFile(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return false;
            }
            if (imageFile.Length > _configuration.GetValue<int>("settings:MaxImageFileSize"))
            {
                return false;
            }
            List<string> allowedMimeTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };
            if (!allowedMimeTypes.Contains(imageFile.ContentType.ToLowerInvariant()))
            {
                return false;
            }

            return true;
        }

        public bool ValidateMarkdownFile(IFormFile markdownFile)
        {
            if (markdownFile == null || markdownFile.Length == 0)
            {
                return false;
            }

            if (markdownFile.Length > 5120)
            {
                return false;
            }

            var allowedMimeType = "text/markdown";
            if (markdownFile.ContentType.ToLowerInvariant() != allowedMimeType)
            {
                return false;
            }

            return true;
        }




    }
}
