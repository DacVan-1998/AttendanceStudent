using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.FaceRecognizer;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Database.Configurations;
using AttendanceStudent.File.Interfaces;
using AttendanceStudent.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<ResourceConfiguration> _resourceConfiguration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecognizerEngine _recognizerEngine;

        public FileController(ILogger<FileController> logger, IFileManagementService fileManagementService, IStringLocalizationService localizationService, IOptions<ResourceConfiguration> resourceConfiguration, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IRecognizerEngine recognizerEngine)
        {
            _logger = logger;
            _fileManagementService = fileManagementService;
            _localizationService = localizationService;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _recognizerEngine = recognizerEngine;
            _resourceConfiguration = resourceConfiguration;
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

                var resources = new Dictionary<IFormFile, StudentImage>();
                foreach (var file in files)
                {
                    var fileName = $"{Guid.NewGuid()}-{Utils.File.GenerateFileName(Path.GetFileNameWithoutExtension(file.FileName))}{Path.GetExtension(file.FileName)}";
                    resources.Add(file, new StudentImage()
                    {
                        Id = Guid.NewGuid(),
                        Name = fileName,
                        Size = file.Length,
                        ContentType = file.ContentType,
                        OriginalName = file.FileName,
                        StudentId = studentId
                    });
                }

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

        /// <summary>
        /// To get all student images filter by student id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllStudentImages")]
        public async Task<IActionResult> GetAllStudentImagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileManagementService.GetStudentImagesAsync(cancellationToken);
                if (result.Succeeded)
                    return Ok(result);
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "GetAllStudentImagesAsync");
                throw;
            }
        }

        /// <summary>
        /// To get all student images by student id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StudentImages/{studentId}")]
        public async Task<IActionResult> GetStudentImagesByStudentIdAsync(Guid studentId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileManagementService.GetStudentImagesByStudentIdAsync(studentId,cancellationToken);
                if (result.Succeeded)
                    return Ok(result);
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "GetStudentImagesByStudentIdAsync");
                throw;
            }
        }
    }
}