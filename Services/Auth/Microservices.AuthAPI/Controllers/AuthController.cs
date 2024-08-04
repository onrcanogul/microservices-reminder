using Microservices.AuthAPI.Dtos;
using Microservices.AuthAPI.Services.Abstracts;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : CustomBaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto login)
            => ControllerResponse(await authService.LoginAsync(login.UsernameOrEmail, login.Password));
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
            => ControllerResponse(await authService.RegisterAsync(userDto));
        [HttpPost("refresh-token-login")]
        public async Task<IActionResult> RefreshTokenLogin(string refreshToken)
            => ControllerResponse(await authService.RefreshTokenLoginAsync(refreshToken));
    }
}
