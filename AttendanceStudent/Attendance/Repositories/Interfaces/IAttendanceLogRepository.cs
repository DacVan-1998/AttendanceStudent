using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Attendance.Repositories.Interfaces
{
    public interface IAttendanceLogRepository: IRepository<Models.AttendanceLog>
    {
        /// <summary>
        /// Get attendance log by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.AttendanceLog?> GetAttendanceLogByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get attendance log by date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.AttendanceLog?> GetAttendanceLogByDateAsync(string dateTime, CancellationToken cancellationToken = default(CancellationToken));

    }
}