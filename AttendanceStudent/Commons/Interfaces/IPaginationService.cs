using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.DTO.Pagination.Responses;

namespace AttendanceStudent.Commons.Interfaces
{
    public interface IPaginationService
    {
        Task<PaginationBaseResponse<T>> PaginateAsync<T>(IQueryable<T> source, int page, string? orderBy, bool orderByDesc, int pageSize, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}