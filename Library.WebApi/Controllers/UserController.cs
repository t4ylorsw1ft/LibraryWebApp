using Library.Application.DTOs.Users;
using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDto">User registration data.</param>
        /// <returns>A JWT pair (access and refresh tokens) upon successful registration.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<JwtPairDto>> Register([FromBody] RegisterDto registerDto)
        {
            var jwtPair = await _userService.RegisterAsync(registerDto);
            return Ok(jwtPair);
        }

        /// <summary>
        /// Login a user and return a JWT pair.
        /// </summary>
        /// <param name="loginDto">User login data.</param>
        /// <returns>A JWT pair (access and refresh tokens) upon successful login.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<JwtPairDto>> Login([FromBody] LoginDto loginDto)
        {
            var jwtPair = await _userService.LoginAsync(loginDto);
            return Ok(jwtPair);
        }

        /// <summary>
        /// Refresh the user's JWT pair using a refresh token.
        /// </summary>
        /// <param name="refreshTokenDto">Data containing the refresh token.</param>
        /// <returns>A new JWT pair (access and refresh tokens) upon successful refresh.</returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<JwtPairDto>> Refresh([FromBody] RefreshDto refreshTokenDto)
        {
            var jwtPair = await _userService.RefreshAsync(refreshTokenDto);
            return Ok(jwtPair);
        }
    }
}
