using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Http;

namespace AttendanceStudent.Attendance.Interfaces
{
    public interface IAttendanceService
    {
        /// <summary>
        /// Create attendance log  
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="resources"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UploadAttendanceLogImageResponse>> CreateAttendanceLogAsync(Guid rollCallId,Dictionary<IFormFile, AttendanceLogImage> resources, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// attendance student
        /// </summary>
        /// <param name="attendanceLogId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> AttendanceStudentsAsync(Guid attendanceLogId,CreateAttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken));

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