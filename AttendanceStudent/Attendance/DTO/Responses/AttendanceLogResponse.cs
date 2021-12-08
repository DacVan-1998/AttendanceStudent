using System;
using System.Collections.Generic;
using AttendanceStudent.Attendance.DTO.Requests;
using AttendanceStudent.Models;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class AttendanceLogResponse
    {
        public Guid Id { get; set; }
        public string AttendanceDate { get; set; } = "";
        public Guid RollCallId { get; set; }
        public string SubjectCode { get; set; } = "";
        public string ClassCode { get; set; } = "";

        public ICollection<AttendanceLogImageViewResponse> LogImages { get; set; } = new List<AttendanceLogImageViewResponse>();
        public ICollection<AttendanceStudentViewResponse> AttendanceStudents = new List<AttendanceStudentViewResponse>();
    }
}