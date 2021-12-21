using System;
using System.Collections.Generic;

namespace AttendanceStudent.File.DTO.Responses
{
    public class ViewStudentImageResponse
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string DictionaryPath { get; set; }
        public List<string> StudentImagePaths { get; set; }
    }
}