using Blog.ReponseExceptions;
using Azure.Storage.Blobs;
using Org.BouncyCastle.Security;
using System.Runtime.InteropServices;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs.Models;

namespace Blog.Services.FileService
{
    public enum FileType
    {
        Md,
        Img
    }
    public class FileService : IFileService
    {
        private readonly IFileValidationService _fileValidationService;
        private BlobContainerClient _mdContainerClient;
        private BlobContainerClient _imagesContainerClient;
        private BlobServiceClient _blobServiceClient;
        public FileService(IFileValidationService fileValidationService)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONNECTION_STRING");
            this._blobServiceClient = new BlobServiceClient(connectionString);
            this._mdContainerClient = this._blobServiceClient.GetBlobContainerClient("markdown-files");
            this._imagesContainerClient = this._blobServiceClient.GetBlobContainerClient("images");
            this._fileValidationService = fileValidationService;

        }


 

        private async Task<string> UploadFile(FileType type, IFormFile file)
        {
            string extension = (type == FileType.Md) ? ".md" : Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName) + Guid.NewGuid().ToString() + extension;
            var stream = file.OpenReadStream();
            BlobHttpHeaders headers = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };
            BlobClient blobClient = (type == FileType.Md) ? this._mdContainerClient.GetBlobClient(fileName) : this._imagesContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = headers });
        
            return blobClient.Uri.AbsoluteUri;

        }

        public async Task<string> UploadMarkdownFile(IFormFile markdownFile)
        {

            if (!this._fileValidationService.ValidateMarkdownFile(markdownFile)) throw new ResponseExceptions.BadRequestException("Please upload a valid markdown file that has a 10Mb limit!");
            string uri;
            try { 
                uri = await this.UploadFile(FileType.Md, markdownFile);
            }
            catch(Exception e)
            {
                throw new ResponseExceptions.BaseResponseException("An error occured when communicating with the blob storage", ResponseExceptions.StatusCodes.INTERNAL_SERVER_ERROR); 
            }
            return uri;
        }

        public async Task<IEnumerable<string>> UploadImageFiles(IEnumerable<IFormFile> imageFiles)
        {

            foreach (var file in imageFiles)
            {
                if (!this._fileValidationService.ValidateImageFile(file)) throw new ResponseExceptions.BadRequestException("Please upload a valid image file that has a 10Mb limit!");
            }
            
            List<string> result = new List<string>();
            try
            {
                foreach (var file in imageFiles)
                {
                    string uri = await this.UploadFile(FileType.Img, file);
                    result.Add(uri);
                }
            }catch(Exception e) {
                await this.DeleteFiles(result);
                throw new ResponseExceptions.BaseResponseException("An error occured when communicating with the blob storage", ResponseExceptions.StatusCodes.INTERNAL_SERVER_ERROR);
            }
        
            return result;
        }


        public async Task<string> UploadImageFile(IFormFile imageFile)
        {
            List<IFormFile> files = new List<IFormFile>();
            files.Add(imageFile);
            List<string> list = (List<string>) await this.UploadImageFiles(files);
            return list[0];

        }

        public async Task<bool> DeleteFile(string fileUri)
        {
            try
            {
                Uri blobUri = new Uri(fileUri);
                string containerName = blobUri.Segments[1];
                string blobName = blobUri.Segments.Last();

                // Get a reference to the blob
                var containerClient = this._blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                // Delete the blob
                var response = await blobClient.DeleteIfExistsAsync();

                return response.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteFiles(IEnumerable<string> fileUris)
        {
            bool result = true;
            foreach(string uri in fileUris)
            {
                result &= await this.DeleteFile(uri);
            }
            return result;

        }
    }
}
