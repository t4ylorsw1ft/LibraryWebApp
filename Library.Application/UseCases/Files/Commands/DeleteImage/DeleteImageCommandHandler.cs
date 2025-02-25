using Library.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Files.Commands.DeleteImage
{
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
    {
        private readonly IFileStorageService _fileStorageService;

        public DeleteImageCommandHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            if (!_fileStorageService.DeleteFile(request.FilePath))
                throw new FileNotFoundException(request.FilePath);
        }
    }
}
