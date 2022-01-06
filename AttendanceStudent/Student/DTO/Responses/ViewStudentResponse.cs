using System;

namespace AttendanceStudent.Student.DTO.Responses
{
    public class ViewStudentResponse
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalImages { get; set; }
    }
}