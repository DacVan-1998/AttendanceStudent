using System;
using System.Collections.Generic;

namespace AttendanceStudent.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<StudentImage> Images = new List<StudentImage>();   
        public ICollection<AttendanceStudent> AttendanceStudents = new List<AttendanceStudent>();
        public ICollection<StudentRollCall> StudentRollCalls = new List<StudentRollCall>();
    }
}