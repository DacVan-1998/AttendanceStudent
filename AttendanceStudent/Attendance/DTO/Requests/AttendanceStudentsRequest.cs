using System;
using System.Collections.Generic;

namespace AttendanceStudent.Attendance.DTO.Requests
{
    public class AttendanceStudentsRequest
    {
        public List<AttendanceStudentDto> Students { get; set; }
    }
}