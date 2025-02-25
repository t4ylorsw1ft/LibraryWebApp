using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Files.Queries.GetImage
{
    public record GetImageQuery(string FilePath) : IRequest<byte[]>;
}
