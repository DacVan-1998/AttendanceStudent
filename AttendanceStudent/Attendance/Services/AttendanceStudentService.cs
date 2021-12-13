using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Attendance.Interfaces;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Database.Configurations;
using AttendanceStudent.File.DTO.Responses;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AttendanceStudent.Attendance.Services
{
    public class AttendanceStudentService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizationService _localizationService;
        private readonly ResourceConfiguration _resourceConfiguration;

        public AttendanceStudentService(IUnitOfWork unitOfWork, IStringLocalizationService localizationService, IOptions<ResourceConfiguration> resourceConfiguration)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _resourceConfiguration = resourceConfiguration.Value;
        }


        public async Task<Result<UploadAttendanceLogImageResponse>> CreateAttendanceLogAsync(Guid rollCallId, Dictionary<IFormFile, AttendanceLogImage> resources, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedRollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(rollCallId, cancellationToken);
                if (existedRollCall == null)
                    return Result<UploadAttendanceLogImageResponse>.Fail(LocalizationString.RollCall.NotFound.ToErrors(_localizationService));
               
                var attendanceLog = new AttendanceLog()
                {
                    Id = Guid.NewGuid(),
                    AttendanceDate = DateTime.Now.ToString("d"),
                    RollCallId = rollCallId
                };
                var response = new UploadAttendanceLogImageResponse();

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
                    return Result<UploadAttendanceLogImageResponse>.Fail(response.Errors);
                // Save to the server disk
                foreach (var (formFile, file) in resources)
                {
                    var path = Path.Combine(_resourceConfiguration.UploadAttendanceImageFolderPath, file.Name);
                    await using var stream = new FileStream(path, FileMode.Create);
                    await formFile.CopyToAsync(stream, cancellationToken); //save the file
                    stream.Close();
                    file.AttendanceLogId = attendanceLog.Id;
                    await _unitOfWork.AttendanceLogImages.AddAsync(file, cancellationToken);
                }

                response.AttendanceLogId = attendanceLog.Id;
                await _unitOfWork.AttendanceLogs.AddAsync(attendanceLog,cancellationToken);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                // Save log
                return result <= 0 ? Result<UploadAttendanceLogImageResponse>.Fail(Constants.CannotFinishRequest) : Result<UploadAttendanceLogImageResponse>.Succeed(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> AttendanceStudentsAsync(Guid attendanceLogId, CreateAttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(attendanceLogId, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<ActionResult>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));

                //Check input students list must be existed in student list of roll call in the db
                foreach (var item in request.Students.Where(item => existedAttendanceLog.RollCall != null && existedAttendanceLog.RollCall.StudentRollCalls.All(x => x.StudentId != item)))
                    return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.NotExist, item).ToErrors(_localizationService));

                // Attendance present for input student ids list 
                if (existedAttendanceLog.RollCall != null)
                {
                    var attendanceStudentCreateList = existedAttendanceLog.RollCall.StudentRollCalls.Select(item => new Models.AttendanceStudent()
                    {
                        Id = Guid.NewGuid(),
                        AttendanceLogId = attendanceLogId,
                        StudentId = item.StudentId,
                        IsPresent = request.Students.Any(x => x == item.StudentId),
                        Note = string.Empty
                    }).ToList();

                    existedAttendanceLog.AttendanceStudents = attendanceStudentCreateList;
                }

                _unitOfWork.AttendanceLogs.Update(existedAttendanceLog);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> UpdateAttendanceLogAsync(Guid attendanceLogId, AttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(attendanceLogId, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<ActionResult>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));

                //Check input students list must be existed in student list of roll call in the db
                foreach (var item in request.Students.Where(item => existedAttendanceLog.RollCall != null && existedAttendanceLog.RollCall.StudentRollCalls.All(x => x.StudentId != item.StudentId)))
                    return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.NotExist, item.StudentId).ToErrors(_localizationService));

                var attendanceStudentUpdateList = request.Students.Select(item => new Models.AttendanceStudent()
                {
                    Id = Guid.NewGuid(),
                    AttendanceLogId = existedAttendanceLog.Id,
                    StudentId = item.StudentId,
                    IsPresent = item.IsPresent,
                    Note = item.Note
                }).ToList();

                existedAttendanceLog.AttendanceStudents = attendanceStudentUpdateList;

                _unitOfWork.AttendanceLogs.Update(existedAttendanceLog);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<AttendanceLogResponse>> ViewAttendanceLogAsync(Guid attendanceLogId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(attendanceLogId, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<AttendanceLogResponse>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));

                return Result<AttendanceLogResponse>.Succeed(new AttendanceLogResponse()
                {
                    Id = existedAttendanceLog.Id,
                    AttendanceDate = existedAttendanceLog.AttendanceDate,
                    RollCallId = existedAttendanceLog.RollCallId,
                    SubjectCode = existedAttendanceLog.RollCall?.Subject?.Code ?? string.Empty,
                    ClassCode = existedAttendanceLog.RollCall?.Class?.Code ?? string.Empty,
                    LogImages = (existedAttendanceLog.LogImages ?? new List<AttendanceLogImage>()).Select(li => new AttendanceLogImageViewResponse()
                    {
                        Id = li.Id,
                        Name = li.Name,
                        OriginalName = li.OriginalName,
                        Size = li.Size,
                        ContentType = li.ContentType
                    }).ToList(),
                    AttendanceStudents = existedAttendanceLog.AttendanceStudents.Select(at => new AttendanceStudentViewResponse()
                    {
                        StudentId = at.Id,
                        StudentName = at.Student?.FullName ?? string.Empty,
                        IsPresent = at.IsPresent,
                        Note = at.Note,
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}