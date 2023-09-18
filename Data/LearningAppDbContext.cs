using elearningapp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace elearningapp.Data
{
    public class LearningAppDbContext : DbContext
    {
        public LearningAppDbContext(DbContextOptions<LearningAppDbContext> options) : base(options)
        {
        }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        //public DbSet<Users> Users { get; set;}

        }
    }

