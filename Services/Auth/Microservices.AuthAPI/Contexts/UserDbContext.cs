using Microservices.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Contexts
{
    public class UserDbContext : IdentityDbContext<User, Role, string>
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
