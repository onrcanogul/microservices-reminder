using Microservices.AuthAPI.Dtos;
using Microservices.AuthAPI.Models;
using Microservices.Shared.Dtos;

namespace Microservices.AuthAPI.Services.Abstracts
{
    public interface IAuthService
    {
        Task<ServiceResponse<Token>> LoginAsync(string userNameOrEmail, string password);
        Task<ServiceResponse<Token>> RefreshTokenLoginAsync(string refreshToken);
        Task<ServiceResponse<NoContent>> RegisterAsync(UserDto userDto);
    }
}
