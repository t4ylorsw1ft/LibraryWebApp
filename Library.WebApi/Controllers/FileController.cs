using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Upload a file to the server.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>File path of the uploaded file.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            Console.WriteLine("Controller");
            if (file == null || file.Length == 0)
                return BadRequest("File not uploaded.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var fileData = stream.ToArray();

            var filePath = await _fileService.UploadImageAsync(fileData, file.FileName);
            return Ok(new { filePath });
        }

        /// <summary>
        /// Download a file from the server.
        /// </summary>
        /// <param name="filePath">The path of the file to download.</param>
        /// <returns>The file content for download.</returns>
        [HttpGet("download")]
        public async Task<IActionResult> GetFile([FromQuery] string filePath)
        {
            var fileData = await _fileService.GetImageAsync(filePath);
            return File(fileData, "application/octet-stream", Path.GetFileName(filePath));
        }

        /// <summary>
        /// Delete a file from the server.
        /// </summary>
        /// <param name="filePath">The path of the file to delete.</param>
        /// <returns>OK response when the file is deleted.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string filePath)
        {
            await _fileService.DeleteImageAsync(filePath);
            return Ok();
        }
    }
}
