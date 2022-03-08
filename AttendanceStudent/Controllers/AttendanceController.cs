using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.Interfaces;
using AttendanceStudent.AttendanceLogImages.Interfaces;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.FaceRecognizer;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Utils = AttendanceStudent.Commons.Utils;
using AttendanceStudent.Database.Configurations;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Extensions.Options;

#pragma warning disable 1591
#pragma warning disable 8618
namespace AttendanceStudent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IStringLocalizationService _localizationService;
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IAttendanceLogImageManagementService _attendanceLogImageManagementService;
        private readonly IOptions<ResourceConfiguration> _resourceConfiguration;
        private readonly IRecognizerEngine _recognizerEngine;

        public AttendanceController(ILogger<AttendanceController> logger, IStringLocalizationService localizationService, IAttendanceService attendanceService, IOptions<ResourceConfiguration> resourceConfiguration, IRecognizerEngine recognizerEngine, IAttendanceLogImageManagementService attendanceLogImageManagementService)
        {
            _logger = logger;
            _localizationService = localizationService;
            _attendanceService = attendanceService;
            _resourceConfiguration = resourceConfiguration;
            _recognizerEngine = recognizerEngine;
            _attendanceLogImageManagementService = attendanceLogImageManagementService;
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
        public async Task<IActionResult> CreateAttendanceLogAsync([FromQuery] Guid classId, [FromQuery] Guid subjectId, [FromQuery] DateTime attendanceDate, [FromQuery] string lesson, List<IFormFile> files, CancellationToken cancellationToken)
        {
            try
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (files == null || files.Count == 0)
                    return Accepted(new FailureResponse("File(s) is empty".ToErrors(_localizationService)));

                var resources = new Dictionary<IFormFile, AttendanceLogImage>();
                foreach (var file in files)
                {
                    var fileName = $"{Guid.NewGuid()}-{Utils.File.GenerateFileName(Path.GetFileNameWithoutExtension(file.FileName))}{Path.GetExtension(file.FileName)}";
                    resources.Add(file, new AttendanceLogImage()
                    {
                        Id = Guid.NewGuid(),
                        Name = fileName,
                        Size = file.Length,
                        ContentType = file.ContentType,
                        OriginalName = file.FileName,
                    });
                }

                var result = await _attendanceService.CreateAttendanceLogAsync(classId, subjectId, attendanceDate, lesson, resources, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse(data: result.Data));
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "CreateAttendanceLogAsync");
                throw;
            }
        }

        /// <summary>
        /// Create attendance log with attendance log image
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AttendanceStudent")]
        [Produces("application/json")]
        public async Task<IActionResult> AttendanceStudentsAsync([FromQuery] Guid attendanceLogId, CreateAttendanceStudentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _attendanceService.AttendanceStudentsAsync(attendanceLogId, request, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "AttendanceStudentsAsync");
                throw;
            }
        }

        /// <summary>
        /// Update attendance log with list student
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("AttendanceStudent")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateAttendanceStudentsAsync([FromQuery] Guid attendanceLogId, AttendanceStudentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendanceLogAsync(attendanceLogId, request, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "UpdateAttendanceStudentsAsync");
                throw;
            }
        }


        /// <summary>
        /// Create attendance log with attendance log image
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{attendanceLogId:guid}")]
        [Produces("application/json")]
        public async Task<IActionResult> ViewAttendanceLogAsync(Guid attendanceLogId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _attendanceService.ViewAttendanceLogAsync(attendanceLogId, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse(data: result.Data));
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "ViewAttendanceLogAsync");
                throw;
            }
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
        [Route("Image/{fileName}")]
        public async Task<IActionResult> ServeResourceAsync(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var resource = await _attendanceLogImageManagementService.ServeLogImageAsync(fileName, cancellationToken);
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
        /// Delete Attendance Log
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{logId}")]
        public async Task<IActionResult> DeleteAttendanceLogAsync(Guid logId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _attendanceService.DeleteAttendanceLogAsync(logId, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}