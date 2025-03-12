using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.UseCases.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);

            if (author == null)
                throw new NotFoundException(typeof(Author), request.Id);

            await _authorRepository.DeleteAsync(author, cancellationToken);
        }
    }
}
