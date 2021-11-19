using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Class> Classes { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentImage> Images { get; set; }
        public DbSet<RollCall> RollCalls { get; set; }
        public DbSet<Models.AttendanceStudent> AttendanceStudents { get; set; }
        
        public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}