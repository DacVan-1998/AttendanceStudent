using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.RollCall.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.RollCall.Repositories.Implements
{
    public class RollCallRepository : Repository<Models.RollCall>, IRollCallRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public RollCallRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Models.RollCall?> GetRollCallByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.RollCalls
                .Include(rc => rc.Class)
                .Include(rc => rc.Subject)
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IQueryable<Models.RollCall>> SearchRollCall(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.CompletedTask;
            var keyword = query.Keyword?.ToUpper() ?? string.Empty;
            return _applicationDbContext.RollCalls
                .Include(rc=>rc.Class)
                .Include(rc=>rc.Subject)
                .Where(r => keyword.Length == 0 || r.Class.Code.Contains(keyword) || r.Subject.Code.Contains(keyword) )
                .AsSplitQuery();
        }
    }
}