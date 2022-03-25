using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Student.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.Student.Repositories.Implements
{
    public class StudentRepository : Repository<Models.Student>, IStudentRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public StudentRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Models.Student?> GetStudentByCodeAsync(string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Where(r => r.StudentCode == code).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Models.Student>> GetStudentsByCodesAsync(List<string> codes, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Where(r => codes.Contains(r.StudentCode)).AsSplitQuery().ToListAsync(cancellationToken);
        }

        public async Task<Models.Student?> GetStudentByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Where(r => r.Email == email).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Student?> GetStudentByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Where(r => r.PhoneNumber == phoneNumber).AsSplitQuery().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Models.Student?> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Include(st => st.Images).AsSplitQuery().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> IsDuplicatedCodeAsync(Guid studentId, string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.AnyAsync(u => u.StudentCode == code && u.Id != studentId, cancellationToken);
        }

        public async Task<bool> IsDuplicatedEmailAsync(Guid studentId, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.AnyAsync(u => u.Email == email && u.Id != studentId, cancellationToken);
        }

        public async Task<bool> IsDuplicatedPhoneNumberAsync(Guid studentId, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.AnyAsync(u => u.PhoneNumber == phoneNumber && u.Id != studentId, cancellationToken);
        }

        public async Task<IQueryable<Models.Student>> SearchStudent(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.CompletedTask;
            var keyword = query.Keyword?.ToUpper() ?? string.Empty;
            return _applicationDbContext.Students
                .Include(st => st.Images)
                .Where(r => keyword.Length == 0 || r.StudentCode.Contains(keyword) || r.FullName.Contains(keyword))
                .AsSplitQuery().AsQueryable();
            ;
        }

        public async Task<List<Models.Student>?> GetStudentsAsync(List<Guid> studentIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Students.Where(r => studentIds.Contains(r.Id))
                .AsSplitQuery().ToListAsync(cancellationToken);
        }
    }
}