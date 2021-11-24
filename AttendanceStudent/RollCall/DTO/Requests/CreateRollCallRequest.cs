using System;

namespace AttendanceStudent.RollCall.DTO.Requests
{
    public class CreateRollCallRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; } = "";
        public string FinishTime { get; set; } = "";
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
    }
}