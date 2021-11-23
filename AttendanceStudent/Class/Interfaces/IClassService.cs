using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Models;

namespace AttendanceStudent.Class.Interfaces
{
    public interface IClassService
    {
        /// <summary>
        /// Create class 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> CreateClassAsync(CreateClassRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> UpdateClassAsync(Guid id, UpdateClassRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> DeleteClassAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// View class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ViewClassResponse>> ViewClassAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// View list class 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PaginationBaseResponse<ViewClassResponse>>> ViewListClassAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

    }
}