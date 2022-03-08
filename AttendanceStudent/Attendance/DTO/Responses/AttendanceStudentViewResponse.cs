using System;
using System.Collections.Generic;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class AttendanceStudentViewResponse
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public string StudentCode { get; set; } = "";
        public bool IsPresent { get; set; }
        public string Note { get; set; } = "";
        public int TotalAbsent { get; set; } = 0;
        public List<ViewPre7DayStatusResponse> Previous7DayStatus { get; set; }
    }
}