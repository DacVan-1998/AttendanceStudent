using System.Collections.Generic;

namespace AttendanceStudent.Database.Configurations
{
    /// <summary>
    /// To manage how Resources can be managed, uploaded and served in the system
    /// </summary>
    public class ResourceConfiguration
    {
        public string UploadFolderPath { get; set; }
        public string ImportExcelFileFolder { get; set; }
        public string FaceLoad { get; set; }
        public string UploadAttendanceImageFolderPath { get; set; }
        public string UploadTemporaryStudentImageFolderPath { get; set; }
        public int MaxFileSize { get; set; }
        public List<string> AllowedExtensions { get; set; }
        public List<string> AllowedContentTypes { get; set; }
        public ClamAvConfiguration ClamAv { get; set; }
        public string UploadExcelFileFolderPath { get; set; }
        public string ExportExcelFileFolderPath { get; set; }
        public List<string> VideoExtNeedToGenerateThumbnail { get; set; }
    }
}