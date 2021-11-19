using System;

namespace AttendanceStudent.Models
{
    public class AttendanceLogImage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        
        public Guid AttendanceLogId { get; set; }
        public AttendanceLog? AttendanceLog { get; set; }
    }
}