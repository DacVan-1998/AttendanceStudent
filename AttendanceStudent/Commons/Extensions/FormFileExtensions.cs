using System.IO;
using AttendanceStudent.Database.Configurations;
using Microsoft.AspNetCore.Http;

namespace AttendanceStudent.Commons.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsAllowedExtension(this IFormFile file, ResourceConfiguration configuration)
        {
            return configuration.AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        public static bool IsAllowedContentType(this IFormFile file, ResourceConfiguration configuration)
        {
            return configuration.AllowedContentTypes.Contains(file.ContentType);
        }

        public static bool IsOverMaxSize(this IFormFile file, ResourceConfiguration configuration)
        {
            return file.Length > configuration.MaxFileSize * 1024 * 1024;
        }

        public static bool IsVideo(this IFormFile file, ResourceConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(file.FileName)) return false;
            return configuration.VideoExtNeedToGenerateThumbnail.Contains(Path.GetExtension(file.FileName).ToLower());
        }
        
    }
}