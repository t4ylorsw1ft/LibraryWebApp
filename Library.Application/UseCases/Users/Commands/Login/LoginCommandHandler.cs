using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Domain.Interfaces.Repositories;
using Library.Application.Interfaces.Security;
using Library.Application.UseCases.Users.DTOs;
using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JwtPairDto>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public LoginCommandHandler(IPasswordHasher passwordHasher, 
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IValidator<LoginDto> loginDtoValidator)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _loginDtoValidator = loginDtoValidator;
        }

        public async Task<JwtPairDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = request.loginDto;

            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByEmail(loginDto.Email, cancellationToken);

            if (user == null || !_passwordHasher.Verify(loginDto.Password, user.PasswordHash))
                throw new LoginException();

            string accessToken = _jwtProvider.GenerateAccessToken(user);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
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
