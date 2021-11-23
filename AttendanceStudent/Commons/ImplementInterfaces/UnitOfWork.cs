using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Class.Repositories.Implements;
using AttendanceStudent.Class.Repositories.Interfaces;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Commons.ImplementInterfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public IClassRepository Classes { get; }

        public UnitOfWork(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Classes = new ClassRepository(_applicationDbContext);
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