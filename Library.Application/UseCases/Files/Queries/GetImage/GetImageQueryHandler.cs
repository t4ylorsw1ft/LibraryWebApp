using Library.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Files.Queries.GetImage
{
    public class GetImageQueryHandler : IRequestHandler<GetImageQuery, byte[]>
    {
        private readonly IFileStorageService _fileStorageService;

        public GetImageQueryHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<byte[]> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            return await _fileStorageService.GetFileAsync(request.FilePath, cancellationToken);
        }
    }
}
