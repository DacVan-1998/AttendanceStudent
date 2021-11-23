using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Class.Repositories.Interfaces;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Class.Repositories.Implements
{
    public class ClassRepository : Repository<Models.Class>, IClassRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ClassRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Models.Class?> GetClassByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Classes.Where(r => r.Name == name).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Class?> GetClassByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Classes.Where(r => r.Code == code).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Class?> GetClassByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Classes.AsSplitQuery().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> IsDuplicatedCodeAsync(Guid classId, string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Classes.AnyAsync(u => u.Code == code && u.Id != classId, cancellationToken);
        }

        public async Task<bool> IsDuplicatedNameAsync(Guid classId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Classes.AnyAsync(u => u.Name == name && u.Id != classId, cancellationToken);
        }

        public async Task<IQueryable<Models.Class>> SearchClassByCode(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.CompletedTask;
            var keyword = query.Keyword?.ToUpper() ?? string.Empty;
            return _applicationDbContext.Classes
                .Where(r => keyword.Length == 0 || r.Code.Contains(keyword))
                .AsSplitQuery();
        }
    }
}