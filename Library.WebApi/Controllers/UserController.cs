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

        [HttpPost("register")]
        public async Task<ActionResult<JwtPairDto>> Register([FromBody] RegisterDto registerDto)
        {
            var jwtPair = await _userService.RegisterAsync(registerDto);
            return Ok(jwtPair);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtPairDto>> Login([FromBody] LoginDto loginDto)
        {
            var jwtPair = await _userService.LoginAsync(loginDto);
            return Ok(jwtPair);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<JwtPairDto>> Refresh([FromBody] RefreshDto refreshTokenDto)
        {
            var jwtPair = await _userService.RefreshAsync(refreshTokenDto);
            return Ok(jwtPair);
        }
    }
}

