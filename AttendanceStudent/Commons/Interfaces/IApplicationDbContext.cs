using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AttendanceStudent.Commons.Interfaces
{
    /// <summary>
    /// Interface for application db context.
    /// Extending IDisposable interface to allow us to dispose application db context in unit of work
    /// </summary>
    public interface IApplicationDbContext : IDisposable
    {
        #region DbSet
        public DbSet<AttendanceStudent.Models.Class> Classes { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<AttendanceStudent.Models.Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentImage> Images { get; set; }
        public DbSet<AttendanceStudent.Models.RollCall> RollCalls { get; set; }
        public DbSet<AttendanceStudent.Models.AttendanceStudent> AttendanceStudents { get; set; }
       
        #endregion
        public DatabaseFacade Database { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}