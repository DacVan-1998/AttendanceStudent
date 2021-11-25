using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Student.DTO.Requests;
using AttendanceStudent.Student.DTO.Responses;

namespace AttendanceStudent.Student.Interfaces
{
    public interface IStudentService
    {
        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> CreateStudentAsync(CreateStudentRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> UpdateStudentAsync(Guid id,UpdateStudentRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete Student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> DeleteStudentAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// View Student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ViewStudentResponse>> ViewStudentAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// View list student 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PaginationBaseResponse<ViewStudentResponse>>> ViewListStudentAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

    }
}