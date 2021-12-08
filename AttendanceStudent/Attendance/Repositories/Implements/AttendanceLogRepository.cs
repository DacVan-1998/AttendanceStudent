using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.Repositories.Interfaces;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Attendance.Repositories.Implements
{
    public class AttendanceLogRepository : Repository<Models.AttendanceLog>, IAttendanceLogRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        
        public AttendanceLogRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<AttendanceLog?> GetAttendanceLogByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.AttendanceLogs
                .Include(al=>al.RollCall)
                .ThenInclude(rc=>rc!.StudentRollCalls)
                .AsSplitQuery().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}