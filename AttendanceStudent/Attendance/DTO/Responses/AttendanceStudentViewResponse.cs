using System;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class AttendanceStudentViewResponse
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public bool IsPresent { get; set; }
        public string Note { get; set; } = "";
    }
}