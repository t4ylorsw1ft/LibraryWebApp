using Library.Application.Interfaces.Services;
using Library.Application.UseCases.Files.Commands.DeleteImage;
using Library.Application.UseCases.Files.Commands.UploadImage;
using Library.Application.UseCases.Files.Queries.GetImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Library.WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Upload a file to the server.
        /// Requires Admin role.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>File path of the uploaded file.</returns>
        [Authorize("AdminPolicy")]
        [HttpPost()]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not uploaded.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            var fileData = stream.ToArray();

            var filePath = await _mediator.Send(new UploadImageCommand(fileData, file.FileName), cancellationToken);
            return Ok(new { filePath });
        }

        /// <summary>
        /// Download a file from the server.
        /// </summary>
        /// <param name="filePath">The path of the file to download.</param>
        /// <returns>The file content for download.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetFile([FromQuery] string filePath, CancellationToken cancellationToken)
        {
            var fileData = await _mediator.Send(new GetImageQuery(filePath), cancellationToken);
            return File(fileData, "application/octet-stream", Path.GetFileName(filePath));
        }

        /// <summary>
        /// Delete a file from the server.
        /// Requires Admin role.
        /// </summary>
        /// <param name="filePath">The path of the file to delete.</param>
        /// <returns>OK response when the file is deleted.</returns>
        [Authorize("AdminPolicy")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteFile([FromQuery] string filePath, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteImageCommand(filePath), cancellationToken);
            return Ok();
        }
    }
}
