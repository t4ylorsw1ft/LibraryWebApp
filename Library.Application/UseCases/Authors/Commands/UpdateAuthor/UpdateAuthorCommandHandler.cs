using AutoMapper;
using FluentValidation;
using Library.Application.DTOs.Authors;
using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.UseCases.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDetailsDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAuthorDto> _validator;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper, IValidator<UpdateAuthorDto> validator)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<AuthorDetailsDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.UpdateAuthorDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var author = _mapper.Map<Author>(request.UpdateAuthorDto);
            await _authorRepository.UpdateAsync(author, cancellationToken);

            return _mapper.Map<AuthorDetailsDto>(author);
        }
    }
}
