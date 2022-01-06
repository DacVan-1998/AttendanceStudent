using System;

namespace AttendanceStudent.RollCall.DTO.Requests
{
    public class UpdateRollCallRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
    }
}