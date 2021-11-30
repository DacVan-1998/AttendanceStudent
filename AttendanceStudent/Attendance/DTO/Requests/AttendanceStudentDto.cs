using System;

namespace AttendanceStudent.Attendance.DTO.Requests
{
    public class AttendanceStudentDto
    {
        public Guid StudentId { get; set; }
        public bool IsPresent { get; set; }
        public string Note { get; set; } = "";
    }
}