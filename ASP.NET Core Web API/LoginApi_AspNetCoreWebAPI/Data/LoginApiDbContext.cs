using Microsoft.EntityFrameworkCore;
using LoginApi_AspNetCoreWebAPI.Models;


namespace LoginApi_AspNetCoreWebAPI.Data
{
    public class LoginApiDbContext: DbContext
    {
        public LoginApiDbContext(DbContextOptions<LoginApiDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
