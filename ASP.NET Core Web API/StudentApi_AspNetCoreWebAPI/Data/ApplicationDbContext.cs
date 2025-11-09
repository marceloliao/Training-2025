using Microsoft.EntityFrameworkCore;
using StudentApi_AspNetCoreWebAPI.Models;

namespace StudentApi_AspNetCoreWebAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Teacher> Teachers { get; set; } = null!;

        public DbSet<Course> Courses { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }       
    }
}
