using C__LINQ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=MONPSEMALIAO01; Initial Catalog=SchoolDb; Integrated Security=true; TrustServerCertificate=true");
        //    //Data Source = MONPSEMALIAO01; Initial Catalog = LoginApiProjectDb; Integrated Security = true; TrustServerCertificate = true;

        //    //optionsBuilder.UseSqlServer("Server=localhost;Database=MyDatabase;User Id=myUsername;Password=myPassword;");
        //}

    }
}
