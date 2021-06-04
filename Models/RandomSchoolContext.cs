using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudAPIWithRepositoryPattern.Models
{
    public class RandomSchoolContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public RandomSchoolContext(DbContextOptions<RandomSchoolContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DbSet<Person> Person { get; set; }
        public DbSet<PersonSkills> PersonSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<Skill>().HasData(
                new Skill() { Id = 1, Name = "C Sharp" },
                new Skill() { Id = 2, Name = "Php" },
                new Skill() { Id = 3, Name = "Angular" },
                new Skill() { Id = 4, Name = "Reactjs" },
                new Skill() { Id = 5, Name = "Javascript" },
                new Skill() { Id = 6, Name = "Jquery" },
                new Skill() { Id = 7, Name = "React Native" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
