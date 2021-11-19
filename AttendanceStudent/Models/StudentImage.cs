using System;

namespace AttendanceStudent.Models
{
    public class StudentImage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
    }
}