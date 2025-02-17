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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            Console.WriteLine("Controller");
            if (file == null || file.Length == 0)
                return BadRequest("Файл не загружен");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var fileData = stream.ToArray();


            var filePath = await _fileService.UploadImageAsync(fileData, file.FileName);
            return Ok(new { filePath });
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetFile([FromQuery] string filePath)
        {
            var fileData = await _fileService.GetImageAsync(filePath);
            return File(fileData, "application/octet-stream", Path.GetFileName(filePath));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string filePath)
        {
            await _fileService.DeleteImageAsync(filePath);
            return Ok();
        }
    }
}
