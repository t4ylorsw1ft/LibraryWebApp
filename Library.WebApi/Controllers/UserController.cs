using Library.Application.Interfaces.Services;
using Library.Application.UseCases.Users.Commands.Login;
using Library.Application.UseCases.Users.Commands.Refresh;
using Library.Application.UseCases.Users.Commands.Register;
using Library.Application.UseCases.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDto">User registration data.</param>
        /// <returns>A JWT pair (access and refresh tokens) upon successful registration.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<JwtPairDto>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var jwtPair = await _mediator.Send(new RegisterCommand(registerDto), cancellationToken);
            return Ok(jwtPair);
        }

        /// <summary>
        /// Login a user and return a JWT pair.
        /// </summary>
        /// <param name="loginDto">User login data.</param>
        /// <returns>A JWT pair (access and refresh tokens) upon successful login.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<JwtPairDto>> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var jwtPair = await _mediator.Send(new LoginCommand(loginDto), cancellationToken);
            return Ok(jwtPair);
        }

        /// <summary>
        /// Refresh the user's JWT pair using a refresh token.
        /// </summary>
        /// <param name="refreshTokenDto">Data containing the refresh token.</param>
        /// <returns>A new JWT pair (access and refresh tokens) upon successful refresh.</returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<JwtPairDto>> Refresh([FromBody] RefreshDto refreshTokenDto, CancellationToken cancellationToken)
        {
            var jwtPair = await _mediator.Send(new RefreshCommand(refreshTokenDto), cancellationToken);
            return Ok(jwtPair);
        }
    }
}
