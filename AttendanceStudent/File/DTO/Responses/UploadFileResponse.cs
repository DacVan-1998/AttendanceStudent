using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Application.Common.Models;

namespace AttendanceStudent.File.DTO.Responses
{
    [ExcludeFromCodeCoverage]
    public class UploadFileResponse
    {
        public List<ErrorItem> Errors { get; set; } = new List<ErrorItem>();
        public List<ViewFileResponse> UploadedFiles { get; set; } = new List<ViewFileResponse>();
    }
}