using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.File.Repositories.Interfaces;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.File.Repositories.Implements
{
    public class FileRepository : Repository<StudentImage>, IFileRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public FileRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<StudentImage?> GetFileByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Images.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<StudentImage?> GetFileByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Images.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }

        public async Task<List<StudentImage>> GetAllFileAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _applicationDbContext.Images
                .Include(img => img.Student).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}