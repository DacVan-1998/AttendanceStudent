using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.File.DTO.Responses;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ActionResult = AttendanceStudent.Commons.Models.ActionResult;

namespace AttendanceStudent.File.Interfaces
{
    public interface IFileManagementService
    {
        /// <summary>
        /// To upload a file or multiple files to the server
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UploadFileResponse>> UploadFilesAsync(Dictionary<IFormFile, StudentImage> resources, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// To delete file from the system
        /// </summary>
        /// <param name="resourceId">Resource Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ResourceResult</returns>
        Task<Result<ActionResult>> DeleteFileAsync(Guid resourceId, CancellationToken cancellationToken);

        /// <summary>
        /// To serve file by its name
        /// </summary>
        /// <param name="fileName">Resource Name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>PhysicalFileResult</returns>
        Task<PhysicalFileResult?> ServeFileAsync(string fileName, CancellationToken cancellationToken);

    }
}