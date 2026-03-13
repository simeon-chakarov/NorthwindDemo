using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts.Login;
using NorthwindDemo.Api.Security;

namespace NorthwindDemo.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IJwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
        {
            // Demo-only fake users
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var token = _jwtTokenService.CreateToken(request.Username, "Admin");
                return Ok(new LoginResponseDto(token));
            }

            if (request.Username == "user" && request.Password == "user123")
            {
                var token = _jwtTokenService.CreateToken(request.Username, "User");
                return Ok(new LoginResponseDto(token));
            }

            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials",
                Detail = "Username or password is incorrect.",
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }
}
