using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;

namespace AttendanceStudent.File.Repositories.Interfaces
{
    public interface IFileRepository : IRepository<StudentImage>
    {
        /// <summary>
        /// Get file by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StudentImage?> GetFileByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get file by its name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StudentImage?> GetFileByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));

    }
}