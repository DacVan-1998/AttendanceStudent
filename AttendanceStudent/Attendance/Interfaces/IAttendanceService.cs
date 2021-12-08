using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Commons.Models;

namespace AttendanceStudent.Attendance.Interfaces
{
    public interface IAttendanceService
    {
        /// <summary>
        /// Update attendance students  
        /// </summary>
        /// <param name="attendanceLogId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> UpdateAttendanceLogAsync(Guid attendanceLogId,AttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update attendance students  
        /// </summary>
        /// <param name="attendanceLogId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AttendanceLogResponse>> ViewAttendanceLogAsync(Guid attendanceLogId, CancellationToken cancellationToken = default(CancellationToken));


    }
}