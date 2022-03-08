using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.DTO.Responses;
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
                .Include(al => al.AttendanceStudents)
                .ThenInclude(ats => ats.Student)
                .Include(al => al.LogImages)
                .Include(al => al.RollCall)
                .ThenInclude(rc => rc.Class)
                .Include(al => al.RollCall)
                .ThenInclude(rc => rc.Subject)
                .Include(al => al.RollCall)
                .ThenInclude(rc => rc!.StudentRollCalls)
                .AsSplitQuery().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<ViewPre7DayStatusResponse>> GetPrevious7DayStatus(Guid rollCallId, Guid studentId, DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var result = await _applicationDbContext.AttendanceStudents
                .Include(al => al.AttendanceLog)
                .AsSplitQuery().Where(r => r.AttendanceLog != null && DateTime.Compare(r.AttendanceLog.AttendanceDate, dateTime) < 0 && r.StudentId == studentId && r.AttendanceLog.RollCallId==rollCallId).Take(7).ToListAsync(cancellationToken);
            return result.Select(x => new ViewPre7DayStatusResponse()
            {
                AttendanceDate = x.AttendanceLog.AttendanceDate.ToString("d"),
                AttendanceTime = x.AttendanceLog.AttendanceTime,
                Status = x.IsPresent
            }).ToList();
        }

        public async Task<int> GetAbsentDaysOfStudent(Guid rollCallId, Guid studentId,DateTime dateTimeRequest, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.AttendanceStudents
                .Include(al => al.AttendanceLog)
                .AsSplitQuery().CountAsync(r => r.AttendanceLog != null && r.IsPresent == false && r.AttendanceLog.RollCallId == rollCallId && r.StudentId == studentId && DateTime.Compare(r.AttendanceLog.AttendanceDate,dateTimeRequest) < 0, cancellationToken);
        }
    }
}