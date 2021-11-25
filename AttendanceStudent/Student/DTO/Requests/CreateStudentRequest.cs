namespace AttendanceStudent.Student.DTO.Requests
{
    public class CreateStudentRequest
    {
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}