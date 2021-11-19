using System;

namespace AttendanceStudent.Models
{
    public class StudentRollCall
    {
        public Guid RollCallId { get; set; }
        public RollCall? RollCall { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
    }
}