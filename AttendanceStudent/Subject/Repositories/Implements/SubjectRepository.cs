using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Subject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Subject.Repositories.Implements
{
    public class SubjectRepository : Repository<Models.Subject>, ISubjectRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public SubjectRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Models.Subject?> GetSubjectByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Subjects.Where(r => r.Name == name).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Subject?> GetSubjectByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Subjects.Where(r => r.Code == code).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Subject?> GetSubjectByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Subjects.AsSplitQuery().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> IsDuplicatedCodeAsync(Guid subjectId, string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Subjects.AnyAsync(u => u.Code == code && u.Id != subjectId, cancellationToken);
        }
        
        public async Task<bool> IsDuplicatedNameAsync(Guid subjectId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Subjects.AnyAsync(u => u.Name == name && u.Id != subjectId, cancellationToken);
        }

        public async Task<IQueryable<Models.Subject>> SearchSubjectByCode(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.CompletedTask;
            var keyword = query.Keyword?.ToUpper() ?? string.Empty;
            return _applicationDbContext.Subjects
                .Where(r => keyword.Length == 0 || r.Code.Contains(keyword) || r.Name.Contains(keyword))
                .AsSplitQuery();
        }
    }
}