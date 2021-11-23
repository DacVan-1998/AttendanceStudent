using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AttendanceStudent.Database
{
    public class ApplicationDbContext : DbContext,IApplicationDbContext
    {
        public DbSet<Models.Class> Classes { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentImage> Images { get; set; }
        public DbSet<RollCall> RollCalls { get; set; }
        public DbSet<Models.AttendanceStudent> AttendanceStudents { get; set; }
        public DatabaseFacade Database => base.Database;

        public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}