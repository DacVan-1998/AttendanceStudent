using System;
using System.Diagnostics.CodeAnalysis;

namespace AttendanceStudent.File.DTO.Responses
{
    [ExcludeFromCodeCoverage]
    public class ViewFileResponse
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string Path { get; set; } = "";
    }
}