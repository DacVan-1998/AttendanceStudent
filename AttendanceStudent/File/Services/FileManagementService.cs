using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Database.Configurations;
using AttendanceStudent.File.DTO.Responses;
using AttendanceStudent.File.Interfaces;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ActionResult = AttendanceStudent.Commons.Models.ActionResult;

namespace AttendanceStudent.File.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly ResourceConfiguration _resourceConfiguration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizationService _localizationService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resourceConfiguration"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="localizationService"></param>
        /// <param name="webHostEnvironment"></param>
        public FileManagementService(IOptions<ResourceConfiguration> resourceConfiguration, IUnitOfWork unitOfWork, IStringLocalizationService localizationService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _webHostEnvironment = webHostEnvironment;
            _resourceConfiguration = resourceConfiguration.Value;
        }

        ///<inheritdoc cref="IFileManagementService.UploadFilesAsync"/>
        public async Task<Result<UploadFileResponse>> UploadFilesAsync(Dictionary<IFormFile, StudentImage> resources, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var response = new UploadFileResponse();

                // Validate input
                foreach (var (formFile, _) in resources)
                {
                    // Check name
                    if (formFile.FileName.Length > Constants.FieldLength.TextMaxLength)
                        response.Errors.Add(new ErrorItem()
                        {
                            FieldName = formFile.FileName,
                            Error = _localizationService[LocalizationString.File.FileNameIsTooLong].Value
                        });
                    // Check ext
                    if (!formFile.IsAllowedExtension(_resourceConfiguration))
                        response.Errors.Add(new ErrorItem()
                        {
                            FieldName = formFile.FileName,
                            Error = _localizationService[LocalizationString.File.NotAllowedExtensions].Value
                        });
                    // Check content type
                    if (!formFile.IsAllowedContentType(_resourceConfiguration))
                        response.Errors.Add(new ErrorItem()
                        {
                            FieldName = formFile.FileName,
                            Error = _localizationService[LocalizationString.File.NotAllowedContentTypes].Value
                        });
                    // Check size
                    if (formFile.IsOverMaxSize(_resourceConfiguration))
                        response.Errors.Add(new ErrorItem()
                        {
                            FieldName = formFile.FileName,
                            Error = _localizationService[LocalizationString.File.FileSizeIsTooLarge].Value
                        });
                }

                if (response.Errors.Count > 0)
                    return Result<UploadFileResponse>.Fail(response.Errors, response);
                // Save to the server disk
                foreach (var (formFile, file) in resources)
                {
                    var path = Path.Combine(_resourceConfiguration.UploadFolderPath, file.Name);
                    await using var stream = new FileStream(path, FileMode.Create);
                    await formFile.CopyToAsync(stream, cancellationToken); //save the file
                    stream.Close();
                    response.UploadedFiles.Add(new ViewFileResponse()
                    {
                        Id = file.Id,
                        Name = file.Name,
                        OriginalName = file.OriginalName,
                        ContentType = file.ContentType,
                        Size = file.Size
                    });
                    await _unitOfWork.Files.AddAsync(file, cancellationToken);
                }

                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                // Save log
                return result <= 0 ? Result<UploadFileResponse>.Fail(Constants.CannotFinishRequest) : Result<UploadFileResponse>.Succeed(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteFileAsync(Guid resourceId, CancellationToken cancellationToken)
        {
            try
            {
                // Find resource by name in the db
                var resource = await _unitOfWork.Files.GetFileByIdAsync(resourceId, cancellationToken);
                if (resource == null)
                    return Result<ActionResult>.Fail(_localizationService[LocalizationString.File.NotFound].Value.ToErrors(_localizationService));

                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, _resourceConfiguration.UploadFolderPath, resource.Name);
                // Make sure file is existed, if not PhysicalFileResult with throw exception which we cannot catch in this code block due to Middleware is handling itself.
                if (!System.IO.File.Exists(filePath))
                    return Result<ActionResult>.Fail(_localizationService[LocalizationString.File.NotFound].Value.ToErrors(_localizationService));

                _unitOfWork.Files.Remove(resource);
                System.IO.File.Delete(filePath);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result <= 0 ? Result<ActionResult>.Fail(Constants.CannotFinishRequest) : Result<ActionResult>.Succeed();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PhysicalFileResult?> ServeFileAsync(string fileName, CancellationToken cancellationToken)
        {
            try
            {
                // Find resource by name in the db
                var resource = await _unitOfWork.Files.GetFileByNameAsync(fileName, cancellationToken);

                if (resource == null)
                    return null;

                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, _resourceConfiguration.UploadFolderPath, resource.Name);
                // Make sure file is existed, if not PhysicalFileResult with throw exception which we cannot catch in this code block due to Middleware is handling itself.
                if (!System.IO.File.Exists(filePath))
                    return null;

                // Allow browser to know len of file
                var result = new PhysicalFileResult(filePath, resource.ContentType) {EnableRangeProcessing = true, FileDownloadName = resource.OriginalName};
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}