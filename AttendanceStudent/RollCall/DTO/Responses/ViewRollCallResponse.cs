using System;
using System.Collections.Generic;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Student.DTO.Responses;
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
        
        public List<AttendanceLogResponse> AttendanceLogs { get; set; }
        public List<ViewStudentResponse> Students { get; set; }
    }
}