using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.File.Interfaces;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utils = AttendanceStudent.Commons.Utils;

#pragma warning disable 1591
#pragma warning disable 8618
namespace AttendanceStudent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IStringLocalizationService _localizationService;
        private readonly ILogger<FileController> _logger;
        private readonly IFileManagementService _fileManagementService;

        public FileController(ILogger<FileController> logger, IFileManagementService fileManagementService, IStringLocalizationService localizationService)
        {
            _logger = logger;
            _fileManagementService = fileManagementService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// To serve file by its name
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     GET /File/default_avatar.png
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">File content</response>
        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> ServeResourceAsync(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var resource = await _fileManagementService.ServeFileAsync(fileName, cancellationToken);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (resource == null)
                    return Accepted(new FailureResponse(LocalizationString.File.NotFound.ToErrors(_localizationService)));

                try
                {
                    // Try to send as streaming file
                    var stream = new FileStream(resource.FileName, FileMode.Open);
                    var result = new FileStreamResult(stream, resource.ContentType)
                    {
                        EnableRangeProcessing = true,
                    };
                    return result;
                }
                catch (Exception e)
                {
                    // If we cannot stream it, return as attachment
                    _logger.LogCritical(e, nameof(ServeResourceAsync));
                    return resource;
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "ServeResourceAsync");
                throw;
            }
        }

        /// <summary>
        /// To upload single file or multiple files into the server.
        /// </summary>
        /// <remarks>
        ///
        /// Allowed File Extensions:
        ///
        ///     ".jpg",".png",".jpeg",".pdf",".gif",".tiff",
        ///
        /// Allowed File Content Types:
        ///
        ///     "image/gif","image/tiff","image/jpeg","image/png","image/jpg",
        /// 
        /// Maximum File Size:
        ///     
        ///     25 Mb/file
        /// 
        /// Maximum Payload Size on Request:
        ///
        ///     2 Gb but make sure that you dont do it that way
        ///
        /// Sample Request:
        ///
        ///     POST /File
        ///         Request Body (multipart/form-data):
        ///             files: "file to upload"
        /// Sample Response:
        ///
        ///
        ///     {
        ///         "status":200,
        ///         "message":"Succeeded",
        ///         "data":
        ///                 {
        ///                     "original_file_name1.png":"encoded_file_name_in_server1.png",
        ///                     "original_file_name2.png":"encoded_file_name_in_server2.png"
        ///                 }
        ///     }
        ///
        ///     In which,
        ///         - original_file_name1.png is original name from the Client side. It is used to save file while downloading,
        ///           once user downloads the file, it will be save as original file name.
        ///         - encoded_file_name_in_server1.png is actual file stored in the Server. It's used to access the file via URL or any html code.
        ///             E.g. 
        ///                 &lt;audio controls&gt;
        ///                     &lt;source src="https://localhost:5001/File/b564f688-fe90-4ea8-a605-e74b37b40128-RABKACAAUwBvAGQAYQAg..mp3" type="audio/mpeg" /&gt;
        ///                 &lt;/audio&gt;
        ///
        ///
        ///
        ///
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> UploadResourceAsync([FromQuery] Guid studentId, List<IFormFile> files, CancellationToken cancellationToken)
        {
            try
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (files == null || files.Count == 0)
                    return Accepted(new FailureResponse("File(s) is empty".ToErrors(_localizationService)));
                var resources = files.ToDictionary(file => file, file => new Models.StudentImage()
                {
                    Id = Guid.NewGuid(),
                    Name = $"{Guid.NewGuid()}-{Utils.File.GenerateFileName(Path.GetFileNameWithoutExtension(file.FileName))}{Path.GetExtension(file.FileName)}",
                    Size = file.Length,
                    ContentType = file.ContentType,
                    OriginalName = file.FileName,
                    StudentId = studentId
                });

                var result = await _fileManagementService.UploadFilesAsync(resources, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "UploadResourceAsync");
                throw;
            }
        }

        /// <summary>
        /// To delete resource
        /// </summary>
        /// <param name="fileId">File Id to be deleted</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{fileId}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteResourceAsync(Guid fileId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileManagementService.DeleteFileAsync(fileId, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "DeleteResourceAsync");
                throw;
            }
        }
    }
}