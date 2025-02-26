using AutoMapper;
using FluentValidation;
using Library.Application.Interfaces.Repositories;
using Library.Application.UseCases.Authors.DTOs;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDetailsDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAuthorDto> _validator;

        public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper, IValidator<CreateAuthorDto> validator)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<AuthorDetailsDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.CreateAuthorDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var author = _mapper.Map<Author>(request.CreateAuthorDto);
            await _authorRepository.AddAsync(author, cancellationToken);

            return _mapper.Map<AuthorDetailsDto>(author);
        }
    }
}
