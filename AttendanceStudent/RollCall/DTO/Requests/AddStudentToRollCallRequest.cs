using System;
using System.Collections.Generic;

namespace AttendanceStudent.RollCall.DTO.Requests
{
    public class AddStudentToRollCallRequest
    {
        public List<Guid> Students { get; set; }
    }
}