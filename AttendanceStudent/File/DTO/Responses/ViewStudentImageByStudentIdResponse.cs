using System;
using System.Collections.Generic;

namespace AttendanceStudent.File.DTO.Responses
{
    public class ViewStudentImageByStudentIdResponse
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string DictionaryPath { get; set; }
        public List<ViewFileResponse> StudentImages { get; set; }
    }
}