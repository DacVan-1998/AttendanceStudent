using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Subject.DTO.Requests;
using AttendanceStudent.Subject.DTO.Responses;

namespace AttendanceStudent.Subject.Interfaces
{
    public interface ISubjectService
    {
        /// <summary>
        /// Create Subject 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> CreateSubjectAsync(CreateSubjectRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> DeleteSubjectAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// View class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ViewSubjectResponse>> ViewSubjectAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// View list class 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PaginationBaseResponse<ViewSubjectResponse>>> ViewListSubjectAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

    }
}