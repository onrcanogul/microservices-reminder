using Microservices.AuthAPI.Models;

namespace Microservices.AuthAPI.Services.Abstracts
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
