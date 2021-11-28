using System;

namespace AttendanceStudent.File.DTO.Requests
{
    public class UploadFileRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string OriginalName { get; set; }
        public bool Private { get; set; } = false;
    }
}