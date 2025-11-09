using First_AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace First_AspNetCoreMVC.Data
{

    public class DemoContext : DbContext
    {
        public DbSet<RegistrationModel> Registrations { get; set; }

        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationModel>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RegistrationModel>()
                .Property(m => m.FirstName)
                .IsRequired();

            modelBuilder.Entity<RegistrationModel>()
                .Property(m => m.FirstName)
                .IsRequired();

            modelBuilder.Entity<RegistrationModel>()
                .Property(m => m.Email)
                .IsRequired();
        }

        public async Task Seed()
        {
            this.Database.EnsureCreated();

            if (!this.Registrations.Any())
            {
                var fake = new FakeData();

                var registrations = await fake.GetRegistrations();
                await this.Registrations.AddRangeAsync(registrations);
                await this.SaveChangesAsync();
            }
        }
    }
}
