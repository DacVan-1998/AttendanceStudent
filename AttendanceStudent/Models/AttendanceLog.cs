using System;
using System.Collections.Generic;

namespace AttendanceStudent.Models
{
    public class AttendanceLog
    {
        public Guid Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string AttendanceTime { get; set; }
        public Guid RollCallId { get; set; }
        public RollCall? RollCall { get; set; }
        
        public ICollection<AttendanceLogImage>? LogImages { get; set; }
        public ICollection<AttendanceStudent> AttendanceStudents = new List<AttendanceStudent>();
    }
}