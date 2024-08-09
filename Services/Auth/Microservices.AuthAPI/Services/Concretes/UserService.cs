using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Services.Abstracts;
using Microservices.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Services.Concretes
{
    public class UserService(UserManager<User> userManager) : IUserService
    {
        public async Task UpdateRefreshToken(string refreshToken, User user, DateTime accessTokenEndDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration = accessTokenEndDate.AddMinutes(1);
                await userManager.UpdateAsync(user);
            }
            else throw new NotFoundException("User not found");
        }
    }
}
