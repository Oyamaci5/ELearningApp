using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningApp.Models;

namespace LearningApp.Data
{
    public class LearningAppContext : DbContext
    {
        public LearningAppContext (DbContextOptions<LearningAppContext> options)
            : base(options)
        {
        }

        public DbSet<LearningApp.Models.Users> Users { get; set; } = default!;

        public DbSet<LearningApp.Models.Courses>? Courses { get; set; }

        public DbSet<LearningApp.Models.Assignments>? Assignments { get; set; }

        public DbSet<LearningApp.Models.Enrollments>? Enrollments { get; set; }
    }
}
