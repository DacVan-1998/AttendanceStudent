using System;
using System.Collections.Generic;
using Application.Common.Models;

namespace AttendanceStudent.Attendance.DTO.Responses
{
    public class UploadAttendanceLogImageResponse
    {
        public List<ErrorItem> Errors { get; set; } = new List<ErrorItem>();
        public Guid AttendanceLogId { get; set; }
    }
}