using Microservices.AuthAPI.Dtos;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Services.Abstracts;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Microservices.AuthAPI.Services.Concretes
{
    public class AuthService(ITokenHandler tokenHandler, UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService) : IAuthService
    {

        private async Task<User> FindUser(string userNameOrEmail)
        {
            User? user = await userManager.FindByEmailAsync(userNameOrEmail);
            if (user == null)
            {
                user = await userManager.FindByNameAsync(userNameOrEmail);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
            }
            return user;
        }

        public async Task<ServiceResponse<Token>> LoginAsync(string userNameOrEmail, string password)
        {
            User user = await FindUser(userNameOrEmail);
            SignInResult result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return ServiceResponse<Token>.Failure("email or password not valid", 400);

            Token token = tokenHandler.CreateAccessToken(user);
            await userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration);

            return ServiceResponse<Token>.Success(token, 200);

        }

        public async Task<ServiceResponse<NoContent>> RegisterAsync(UserDto userDto)
        {
            User user = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = userDto.Email,
                UserName = userDto.Username
            };

            IdentityResult identityResult = await userManager.CreateAsync(user, userDto.Password);

            if (!identityResult.Succeeded)
                return ServiceResponse<NoContent>.Failure(identityResult.Errors.Select(e => e.Description).ToList(),400);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status201Created);
        }

        public async Task<ServiceResponse<Token>> RefreshTokenLoginAsync(string refreshToken)
        {
            User? user = await userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
            if (user == null)
                return ServiceResponse<Token>.Failure("user not found", 404);
            
            if(user.RefreshTokenExpiration > DateTime.UtcNow)
            {
                Token token = tokenHandler.CreateAccessToken(user);
                await userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration);

                return ServiceResponse<Token>.Success(token, StatusCodes.Status200OK);
            }
            return ServiceResponse<Token>.Failure("Refresh token is invalid", StatusCodes.Status400BadRequest);

        }

    }
}
