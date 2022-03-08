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
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <param name="attendanceDate"></param>
        /// <param name="lesson"></param>
        /// <param name="resources"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UploadAttendanceLogImageResponse>> CreateAttendanceLogAsync(Guid classId,Guid subjectId,DateTime attendanceDate,string lesson,Dictionary<IFormFile, AttendanceLogImage> resources, CancellationToken cancellationToken = default(CancellationToken));
        
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
        
        /// <summary>
        /// Delete Attendance Log by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> DeleteAttendanceLogAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    }
}