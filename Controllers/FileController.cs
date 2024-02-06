using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Services.FileService;
using Blog.Dto;

namespace Blog.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("markdown")]
        public async Task<ActionResult<string>> UploadMarkdownFile([FromForm] IFormFile markdownFile)
        {
            var result = await _fileService.UploadMarkdownFile(markdownFile);
            return Ok(result);
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
