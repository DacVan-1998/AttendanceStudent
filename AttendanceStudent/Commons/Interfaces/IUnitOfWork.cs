using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Class.Repositories.Interfaces;
using AttendanceStudent.Subject.Repositories.Interfaces;

namespace AttendanceStudent.Commons.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClassRepository Classes { get; }
        ISubjectRepository Subjects { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}