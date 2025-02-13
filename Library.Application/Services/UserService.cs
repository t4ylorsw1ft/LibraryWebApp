using Library.Application.Common.Exceptions;
using Library.Application.DTOs;
using Library.Application.Interfaces.Repositories;
using Library.Application.Interfaces.Security;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJWTProvider _jwtProvider;

        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJWTProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            string passwordHash = _passwordHasher.Generate(password);

            User user = new User()
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<JwtPairDto> LoginAsync(string email, string password)
        {
            User? user = await _userRepository.GetByEmail(email);

            if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
                throw new LoginException();

            string accessToken = _jwtProvider.GenerateAccessToken(user);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userRepository.UpdateAsync(user);

            JwtPairDto jwtPair = new JwtPairDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return jwtPair;
        }

        public async Task<JwtPairDto> RefreshAsync(string refreshToken)
        {
            User? user = await _userRepository.GetByRefreshToken(refreshToken);

            if (user == null)
                throw new NotFoundException(typeof(User), refreshToken);

            string newAccessToken = _jwtProvider.GenerateAccessToken(user);
            string newRefreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user);

            JwtPairDto jwtPair = new JwtPairDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return jwtPair;
        }
    }
}
