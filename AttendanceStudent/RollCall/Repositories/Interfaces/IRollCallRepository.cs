using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.RollCall.Repositories.Interfaces
{
    public interface IRollCallRepository : IRepository<Models.RollCall>
    {
        /// <summary>
        /// Get roll call by its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.RollCall?> GetRollCallByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get roll call by class id and subject id
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Models.RollCall?> GetRollCallByClassAndSubjectAsync(Guid classId, Guid subjectId, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Search for roll call by its name
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Models.RollCall>> SearchRollCall(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken));
    }
}