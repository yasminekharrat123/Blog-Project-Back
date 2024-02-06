using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Services.FileService;
using Blog.Dto;
using Blog.Middleware;

namespace Blog.Controllers
{
    [ApiController]
    [Route("files")]
    [ServiceFilter(typeof(AuthMiddleware))]


    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("images")]
        public async Task<ActionResult<IEnumerable<string>>> UploadImageFiles([FromForm] IEnumerable<IFormFile> imageFiles)
        {
            var result = await _fileService.UploadImageFiles(imageFiles);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> DeleteFiles([FromBody] DeleteFilesDto body )
        {

            var result = await _fileService.DeleteFiles(body.fileUrls);
            return Ok(result);
        }
    }


}
