using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class AppTestContext : IdentityDbContext<CustomIdentityUser>
    {
        public AppTestContext(DbContextOptions<AppTestContext> options) : base(options)
        {
                
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            //apply no action in foreginKey
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
               new Student
               {
                   Id = 1,
                   Name = "Sandeep",
                   Course = "MCA",
                   Subject = "IT",
                   Photo = "dan.jpg"
               },
                new Student
                {
                    Id = 2,
                    Name = "Sandeep2",
                    Course = "MCA",
                    Subject = "IT",
                    Photo = "tan.jpg"
                }
               );
        }
    }
}
