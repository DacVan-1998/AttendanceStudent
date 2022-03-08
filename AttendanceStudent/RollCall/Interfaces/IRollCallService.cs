using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.RollCall.DTO.Requests;
using AttendanceStudent.RollCall.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ActionResult = AttendanceStudent.Commons.Models.ActionResult;

namespace AttendanceStudent.RollCall.Interfaces
{
    public interface IRollCallService
    {
        /// <summary>
        /// Create Roll Call 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> CreateRollCallAsync(CreateRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Roll Call 
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> UpdateRollCallAsync(Guid rollCallId,UpdateRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Delete Roll Call 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> DeleteRollCallAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// View Roll Call 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ViewRollCallResponse>> ViewRollCallAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// View list roll call 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PaginationBaseResponse<ViewRollCallResponse>>> ViewListRollCallAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Add Students to Roll Call 
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> AddStudentToRollCallAsync(Guid rollCallId,AddStudentToRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Remove Students to Roll Call 
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> RemoveStudentToRollCallAsync(Guid rollCallId,RemoveStudentToRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Import Students to Roll Call 
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="excelFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ActionResult>> ImportStudentToRollCallAsync(Guid rollCallId,IFormFile  excelFile, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Export Roll Call 
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<ExportRollCallResponse>> ExportRollCallAsync(Guid rollCallId, CancellationToken cancellationToken = default(CancellationToken));
        
        public Task<PhysicalFileResult> DownloadExportFileAsync(string fileName, CancellationToken cancellationToken = default(CancellationToken));

    }
}