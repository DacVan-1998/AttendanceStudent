using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;

namespace AttendanceStudent.AttendanceLogImages.Repositories.Interfaces
{
    public interface IAttendanceLogImageRepository: IRepository<Models.AttendanceLogImage>
    {
        /// <summary>
        /// Get log image by its name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AttendanceLogImage?> GetLogImageByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}