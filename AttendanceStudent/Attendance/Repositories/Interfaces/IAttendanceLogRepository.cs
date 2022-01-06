using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;

namespace AttendanceStudent.Attendance.Repositories.Interfaces
{
    public interface IAttendanceLogRepository: IRepository<Models.AttendanceLog>
    {
        /// <summary>
        /// Get attendance log by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.AttendanceLog?> GetAttendanceLogByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get attendance log by date
        /// </summary>
        /// <param name="attendanceLogId"></param>
        /// <param name="studentId"></param>
        /// <param name="dateTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        Task<List<ViewPre7DayStatusResponse>> GetPrevious7DayStatus(Guid attendanceLogId,Guid studentId,DateTime dateTime, CancellationToken cancellationToken = default);
    }
}