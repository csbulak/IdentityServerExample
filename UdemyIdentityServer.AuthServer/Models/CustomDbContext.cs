using Microsoft.EntityFrameworkCore;

namespace UdemyIdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser()
                {
                    Id = 1,
                    UserName = "cemalbulak",
                    Email = "cemalbulak41@gmail.com",
                    Password = "123",
                    City = "Kocaeli"
                },
                new CustomUser()
                {
                    Id = 2,
                    UserName = "sumeyyebulak",
                    Email = "sumeyyebulak@gmail.com",
                    Password = "123",
                    City = "Kocaeli"
                },
                new CustomUser()
                {
                    Id = 3,
                    UserName = "goktugbulak",
                    Email = "goktugbulak@gmail.com",
                    Password = "123",
                    City = "Kocaeli"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}