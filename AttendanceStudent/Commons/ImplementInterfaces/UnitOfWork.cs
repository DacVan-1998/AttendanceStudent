using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.Repositories.Implements;
using AttendanceStudent.Attendance.Repositories.Interfaces;
using AttendanceStudent.AttendanceLogImages.Repositories.Implements;
using AttendanceStudent.AttendanceLogImages.Repositories.Interfaces;
using AttendanceStudent.Class.Repositories.Implements;
using AttendanceStudent.Class.Repositories.Interfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.File.Repositories.Implements;
using AttendanceStudent.File.Repositories.Interfaces;
using AttendanceStudent.RollCall.Repositories.Implements;
using AttendanceStudent.RollCall.Repositories.Interfaces;
using AttendanceStudent.Student.Repositories.Implements;
using AttendanceStudent.Student.Repositories.Interfaces;
using AttendanceStudent.Subject.Repositories.Implements;
using AttendanceStudent.Subject.Repositories.Interfaces;

namespace AttendanceStudent.Commons.ImplementInterfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public IClassRepository Classes { get; }
        public ISubjectRepository Subjects { get; }
        public IRollCallRepository RollCalls { get; }
        public IStudentRepository Students { get; }
        public IFileRepository Files { get; }
        public IAttendanceLogRepository AttendanceLogs { get; }
        public IAttendanceLogImageRepository AttendanceLogImages { get; }

        public UnitOfWork(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Classes = new ClassRepository(_applicationDbContext);
            Subjects = new SubjectRepository(_applicationDbContext);
            RollCalls = new RollCallRepository(_applicationDbContext);
            Students = new StudentRepository(_applicationDbContext);
            Files = new FileRepository(_applicationDbContext);
            AttendanceLogs = new AttendanceLogRepository(_applicationDbContext);
            AttendanceLogImages = new AttendanceLogImageRepository(_applicationDbContext);
        }


        public void Dispose()
        {
            _applicationDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        
        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}