using System;
using System.Collections.Generic;
using AttendanceStudent.Models;
using Microsoft.AspNetCore.Http;

namespace AttendanceStudent.Attendance.DTO.Requests
{
    public class CreateAttendanceStudentsRequest
    {
        public List<Guid> Students { get; set; }
    }
}