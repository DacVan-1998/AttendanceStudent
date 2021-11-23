using System;

namespace AttendanceStudent.Subject.DTO.Responses
{
    public class ViewSubjectResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}