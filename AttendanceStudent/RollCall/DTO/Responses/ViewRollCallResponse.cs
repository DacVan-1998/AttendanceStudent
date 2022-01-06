using System;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Subject.DTO.Responses;

namespace AttendanceStudent.RollCall.DTO.Responses
{
    public class ViewRollCallResponse
    {
        public Guid Id { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public ViewClassResponse Class { get; set; }
        public ViewSubjectResponse Subject { get; set; }
    }
}