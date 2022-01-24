using AttendanceStudent.Commons.Attribute;

namespace AttendanceStudent.Student.DTO.Responses
{
    public class StudentConvertExcelDto
    {
        [Column(1)]
        public string StudentCode { get; set; }
        
        [Column(2)]
        public string FullName { get; set; }
        
        [Column(3)]
        public string Email { get; set; }
        
        [Column(4)]
        public string PhoneNumber { get; set; }
    }
}