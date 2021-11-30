using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Attendance.Interfaces;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;

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

        public async Task<Result<ActionResult>> AttendanceStudentsAsync(Guid attendanceLogId, AttendanceStudentsRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedAttendanceLog = await _unitOfWork.AttendanceLogs.GetAttendanceLogByIdAsync(attendanceLogId, cancellationToken);
                if (existedAttendanceLog == null)
                    return Result<ActionResult>.Fail(LocalizationString.AttendanceLog.NotFound.ToErrors(_localizationService));
               
                //Check input categories list must not be existed in categories list of tenant in the db
                foreach (var item in request.Students.Where(item => existedAttendanceLog.RollCall != null && existedAttendanceLog.RollCall.StudentRollCalls.All(x => x.StudentId != item.StudentId)))
                    return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.NotExist, item).ToErrors(_localizationService));

                return Result<ActionResult>.Fail(LocalizationString.Class.DuplicatedName.ToErrors(_localizationService));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}