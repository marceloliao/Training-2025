using Microsoft.EntityFrameworkCore;
using Student_AspNetCoreMVC.Models;

namespace Student_AspNetCoreMVC.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {            
        }
    }
}
