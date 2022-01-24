using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Models;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class AttendanceLogResponse
    {
        public Guid Id { get; set; }
        public string AttendanceDate { get; set; } = "";
        public string AttendanceTime { get; set; } = "";
        public string PresentRate { get; set; } = "";
        public List<string>? LogImagePaths { get; set; }
        public List<AttendanceStudentViewResponse> AttendanceStudents { get; set; }
    }
}