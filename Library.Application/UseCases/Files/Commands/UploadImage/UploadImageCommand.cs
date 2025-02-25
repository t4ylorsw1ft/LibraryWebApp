using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Files.Commands.UploadImage
{
    public record UploadImageCommand(byte[] FileData, string FileName) : IRequest<string>;
}
