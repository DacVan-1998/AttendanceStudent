using System;
using System.Collections.Generic;

namespace AttendanceStudent.RollCall.DTO.Requests
{
    public class RemoveStudentToRollCallRequest
    {
        public List<Guid> Students { get; set; }
    }
}