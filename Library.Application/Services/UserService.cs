using AutoMapper;
using FluentValidation;
using Library.Application.Common.Exceptions;
using Library.Application.DTOs.Users;
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
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public UserService(
            IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IMapper mapper,
            IValidator<RegisterDto> registerDtoValidator,
            IValidator<LoginDto> loginDtoValidator)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _registerDtoValidator = registerDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }

        public async Task<JwtPairDto> RegisterAsync(RegisterDto registerDto)
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            string username = registerDto.Username;
            string email = registerDto.Email;
            string passwordHash = _passwordHasher.Generate(registerDto.Password);

            if (await _userRepository.GetByEmail(email) != null)
                throw new AlreadyExistsException("email");

            User user = new User()
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            var newUser = await _userRepository.AddAsync(user);

            string accessToken = _jwtProvider.GenerateAccessToken(newUser);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            await _userRepository.UpdateAsync(user);

            JwtPairDto jwtPair = new JwtPairDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return jwtPair;
        }

        public async Task<JwtPairDto> LoginAsync(LoginDto loginDto)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            User? user = await _userRepository.GetByEmail(loginDto.Email);

            if (user == null || !_passwordHasher.Verify(loginDto.Password, user.PasswordHash))
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

        public async Task<JwtPairDto> RefreshAsync(RefreshDto refreshDto)
        {
            string refreshToken = refreshDto.RefreshToken;

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
