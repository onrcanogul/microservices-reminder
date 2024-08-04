using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Services.Abstracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.AuthAPI.Services.Concretes
{
    public class TokenHandler(IConfiguration configuration) : ITokenHandler
    {

        public Token CreateAccessToken(User user)
        {
            Token token = new();

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]!));

            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddMinutes(15);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                notBefore: DateTime.UtcNow,
                expires: token.Expiration,
                signingCredentials: signingCredentials,
                claims: new List<Claim> { }
                );

            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
