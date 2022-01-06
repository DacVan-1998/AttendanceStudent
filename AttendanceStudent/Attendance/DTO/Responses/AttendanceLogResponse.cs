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

        public ICollection<string> LogImagePaths = new List<string>();
        public List<AttendanceStudentViewResponse> AttendanceStudents = new List<AttendanceStudentViewResponse>();
        
    }
}