using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ActionResult = AttendanceStudent.Commons.Models.ActionResult;

namespace AttendanceStudent.AttendanceLogImages.Interfaces
{
    public interface IAttendanceLogImageManagementService
    {
      
        /// <summary>
        /// To delete log image from the system
        /// </summary>
        /// <param name="resourceId">Resource Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ResourceResult</returns>
        Task<Result<ActionResult>> DeleteAttendanceLogImageAsync(Guid resourceId, CancellationToken cancellationToken);

        /// <summary>
        /// To serve log image by its name
        /// </summary>
        /// <param name="fileName">Resource Name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>PhysicalFileResult</returns>
        Task<PhysicalFileResult?> ServeLogImageAsync(string fileName, CancellationToken cancellationToken);
        
    }
}