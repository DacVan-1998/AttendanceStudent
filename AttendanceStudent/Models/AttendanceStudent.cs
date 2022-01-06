#nullable enable
using System;

namespace AttendanceStudent.Models
{
    public class AttendanceStudent
    {
        public bool IsPresent { get; set; }
        public string Note { get; set; } = "";
        public Guid AttendanceLogId { get; set; }
        public AttendanceLog? AttendanceLog { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
    }
}