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

namespace Library.Application.UseCases.Users.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, JwtPairDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IJwtValidator _jwtValidator;

        public RefreshCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IJwtValidator jwtValidator)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _jwtValidator = jwtValidator;
        }

        public async Task<JwtPairDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            string refreshToken = request.refreshDto.RefreshToken;

            User? user = await _userRepository.GetByRefreshToken(refreshToken, cancellationToken);

            if (user == null)
                throw new NotFoundException(typeof(User), refreshToken);

            if (_jwtValidator.ValidateTokenByExpiration(refreshToken))
                throw new Exception("Token has expired");

            string newAccessToken = _jwtProvider.GenerateAccessToken(user);
            string newRefreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user, cancellationToken);

            JwtPairDto jwtPair = new JwtPairDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return jwtPair;
        }
    }
}
