#nullable enable
using System;
using System.Collections.Generic;

namespace AttendanceStudent.Models
{
    public class RollCall
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; } = "";
        public string FinishTime { get; set; } = "";
        public Guid ClassId { get; set; }
        public Class? Class { get; set; }
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }
        
        public ICollection<Student> Students = new List<Student>();
        public ICollection<AttendanceLog> AttendanceLogs = new List<AttendanceLog>();
    }
}