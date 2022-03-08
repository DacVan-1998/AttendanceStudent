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


        public async Task<Result<UploadAttendanceLogImageResponse>> CreateAttendanceLogAsync(Guid classId, Guid subjectId, DateTime attendanceDate, string lesson, Dictionary<IFormFile, AttendanceLogImage> resources, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedRollCall = await _unitOfWork.RollCalls.GetRollCallByClassAndSubjectAsync(classId, subjectId, cancellationToken);
                if (existedRollCall == null)
                    return Result<UploadAttendanceLogImageResponse>.Fail(LocalizationString.RollCall.NotFound.ToErrors(_localizationService));

                var attendanceLog = new AttendanceLog()
                {
                    Id = Guid.NewGuid(),
                    AttendanceDate = attendanceDate,
                    AttendanceTime = lesson,
                    RollCallId = existedRollCall.Id
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
                await _unitOfWork.AttendanceLogs.AddAsync(attendanceLog, cancellationToken);
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
                var listStudentIds = new List<Guid>(request.Students.Distinct());
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(attendanceLogId, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<ActionResult>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));

                //Check input students list must be existed in student list of roll call in the db
                foreach (var item in request.Students.Where(item => existedAttendanceLog.RollCall != null && existedAttendanceLog.RollCall.StudentRollCalls.All(x => x.StudentId != item)))
                    listStudentIds.Remove(item);
                // return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.NotExist, item).ToErrors(_localizationService));

                // Attendance present for input student ids list 
                if (existedAttendanceLog.RollCall != null)
                {
                    var attendanceStudentCreateList = existedAttendanceLog.RollCall.StudentRollCalls.Select(item => new Models.AttendanceStudent()
                    {
                        AttendanceLogId = attendanceLogId,
                        StudentId = item.StudentId,
                        IsPresent = listStudentIds.Any(x => x == item.StudentId),
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

                var attendanceStudentList = new List<AttendanceStudentViewResponse>();
                foreach (var item in existedAttendanceLog.AttendanceStudents)
                {
                    attendanceStudentList.Add(new AttendanceStudentViewResponse()
                    {
                        StudentId = item.StudentId,
                        StudentName = item.Student?.FullName ?? string.Empty,
                        StudentCode = item.Student?.StudentCode ?? string.Empty,
                        IsPresent = item.IsPresent,
                        Note = item.Note,
                        TotalAbsent = await _unitOfWork.AttendanceLogs.GetAbsentDaysOfStudent(existedAttendanceLog.RollCallId, item.StudentId,existedAttendanceLog.AttendanceDate, cancellationToken),
                        Previous7DayStatus = await _unitOfWork.AttendanceLogs.GetPrevious7DayStatus(existedAttendanceLog.RollCallId, item.StudentId, existedAttendanceLog.AttendanceDate, cancellationToken),
                    });
                }

                return Result<AttendanceLogResponse>.Succeed(new AttendanceLogResponse()
                {
                    Id = existedAttendanceLog.Id,
                    Class = existedAttendanceLog.RollCall?.Class?.Name ?? string.Empty,
                    Subject = existedAttendanceLog.RollCall?.Subject?.Name ?? string.Empty,
                    AttendanceDate = existedAttendanceLog.AttendanceDate.ToString("d"),
                    AttendanceTime = existedAttendanceLog.AttendanceTime,
                    LogImagePaths = (existedAttendanceLog.LogImages ?? new List<AttendanceLogImage>()).Select(li => Path.Combine("https://localhost:5001/Attendance/Image", li.Name)).ToList(),
                    AttendanceStudents = attendanceStudentList,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteAttendanceLogAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(id, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<ActionResult>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));

                _unitOfWork.AttendanceLogs.Remove(existedAttendanceLog);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}