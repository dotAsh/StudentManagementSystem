using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> students { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().HasData(
                 new Student
                 {
                     Id = 1,
                     Name = "shinchi",
                     Gender = "female",
                     Email = "shinchi@example.com",
                     Department = "Computer Science",
                     EnrollmentDate = DateTime.UtcNow.Date,
                    
                     CreatedDate = DateTimeOffset.UtcNow,
                     UpdatedDate = DateTimeOffset.UtcNow
                 },
                 new Student
                 {
                     Id = 2,
                     Name = "shaon",
                     Gender = "male",
                     Email = "shaon@example.com",
                     Department = "Electrical Engineering",
                     EnrollmentDate = DateTime.Parse("2024-04-28").ToUniversalTime(),
                     CreatedDate = DateTimeOffset.UtcNow,
                     UpdatedDate = DateTimeOffset.UtcNow
                 }


              );

        }
    }
}
