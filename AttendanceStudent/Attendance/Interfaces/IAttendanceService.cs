using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Commons.Models;

namespace AttendanceStudent.Attendance.Interfaces
{
    public interface IAttendanceService
    {
        /// <summary>
        /// Attendance Students  
        /// </summary>
        /// <param name="attendanceLogId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> AttendanceStudentsAsync(Guid attendanceLogId,AttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken));

    }
}