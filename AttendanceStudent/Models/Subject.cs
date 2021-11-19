using System;
using System.Collections.Generic;

namespace AttendanceStudent.Models
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        public ICollection<RollCall> RollCalls = new List<RollCall>();

    }
}