using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.DTOs.Users;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Security;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, JwtPairDto>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDto> _registerDtoValidator;

        public RegisterCommandHandler(IPasswordHasher passwordHasher, 
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IMapper mapper, 
            IValidator<RegisterDto> registerDtoValidator)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _registerDtoValidator = registerDtoValidator;
        }

        public async Task<JwtPairDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDto = request.registerDto;

            var validationResult = await _registerDtoValidator.ValidateAsync(registerDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            string username = registerDto.Username;
            string email = registerDto.Email;
            string passwordHash = _passwordHasher.Generate(registerDto.Password);

            if (await _userRepository.GetByEmail(email, cancellationToken) != null)
                throw new AlreadyExistsException("email");

            User user = new User()
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            var newUser = await _userRepository.AddAsync(user, cancellationToken);

            string accessToken = _jwtProvider.GenerateAccessToken(newUser);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            await _userRepository.UpdateAsync(user, cancellationToken);

            JwtPairDto jwtPair = new JwtPairDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return jwtPair;
        }
    }
}
