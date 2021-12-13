using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.AttendanceLogImages.Repositories.Interfaces;
using AttendanceStudent.Commons.ImplementInterfaces;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceStudent.AttendanceLogImages.Repositories.Implements
{
    public class AttendanceLogImageRepository : Repository<Models.AttendanceLogImage>, IAttendanceLogImageRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        
        public AttendanceLogImageRepository(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}