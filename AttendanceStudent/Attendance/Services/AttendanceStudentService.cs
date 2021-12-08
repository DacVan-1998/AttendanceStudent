using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Attendance.Interfaces;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Models;
using AttendanceStudent.Subject.DTO.Responses;

namespace AttendanceStudent.Attendance.Services
{
    public class AttendanceStudentService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizationService _localizationService;

        public AttendanceStudentService(IUnitOfWork unitOfWork, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
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
                    AttendanceDate = existedAttendanceLog.AttendanceDate.ToString("MM/dd/yyyy"),
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