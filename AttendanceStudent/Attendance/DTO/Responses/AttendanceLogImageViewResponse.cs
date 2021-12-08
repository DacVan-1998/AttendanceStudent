using System;
using AttendanceStudent.Models;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class AttendanceLogImageViewResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string OriginalName { get; set; } = "";
        public long Size { get; set; }
        public string ContentType { get; set; } = "";
    }
}