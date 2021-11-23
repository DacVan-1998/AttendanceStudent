using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Subject.Repositories.Interfaces
{
    public interface ISubjectRepository : IRepository<Models.Subject>
    {
        /// <summary>
        /// Get subject by its name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Subject?> GetSubjectByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Get subject by its code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Subject?> GetSubjectByCodeAsync(string code, CancellationToken cancellationToken = default(CancellationToken));

          
        /// <summary>
        /// Get subject by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Subject?> GetSubjectByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="subjectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedCodeAsync(Guid subjectId,string code, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subjectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedNameAsync(Guid subjectId,string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Search for subject by its name
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Models.Subject>> SearchSubjectByCode(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

    }
}