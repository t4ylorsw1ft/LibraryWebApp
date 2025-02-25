using Library.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Files.Commands.UploadImage
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
    {
        private readonly IFileStorageService _fileStorageService;

        public UploadImageCommandHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(request.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png")
                throw new ArgumentException("Invalid image format");

            return await _fileStorageService.SaveFileAsync(request.FileData, request.FileName, cancellationToken);
        }
    }
}
