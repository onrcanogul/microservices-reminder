using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Models
{
    public class User : IdentityUser<string>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration{ get; set; }
    }
}
