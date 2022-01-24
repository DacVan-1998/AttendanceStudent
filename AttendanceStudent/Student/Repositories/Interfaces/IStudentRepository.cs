using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Student.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Models.Student>
    {
        /// <summary>
        /// Get student by code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Student?> GetStudentByCodeAsync(string code, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get students by codes
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Models.Student>> GetStudentsByCodesAsync(List<string> codes, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Get student by code
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Student?> GetStudentByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get student by code
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Student?> GetStudentByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Student?> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedCodeAsync(Guid studentId, string code, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedEmailAsync(Guid studentId, string email, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedPhoneNumberAsync(Guid studentId, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Search for subject by its name
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Models.Student>> SearchStudent(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Models.Student>?> GetStudentsAsync(List<Guid> studentIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}