using Microservices.AuthAPI.Models;

namespace Microservices.AuthAPI.Services.Abstracts
{
    public interface IUserService
    {
        Task UpdateRefreshToken(string refreshToken, User user, DateTime accessTokenEndDate);
    }
}
