using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Class.Repositories.Interfaces
{
    public interface IClassRepository : IRepository<Models.Class>
    {
        /// <summary>
        /// Get class by its name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Class?> GetClassByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Get class by its code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Class?> GetClassByCodeAsync(string code, CancellationToken cancellationToken = default(CancellationToken));

          
        /// <summary>
        /// Get class by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.Class?> GetClassByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="classId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedCodeAsync(Guid classId,string code, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Check duplicate name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsDuplicatedNameAsync(Guid classId,string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Search for role by its name
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Models.Class>> SearchClassByCode(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));

    }
}