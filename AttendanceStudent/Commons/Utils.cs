using System;

namespace AttendanceStudent.Commons
{
    public static class Utils
    {
        /// <summary>
        /// File service
        /// </summary>
        public static class File
        {
            /// <summary>
            /// To generate base64 file name
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static string GenerateFileName(string fileName)
            {
                var raw = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(fileName));
                return raw.Substring(0, raw.Length > 20 ? 20 : raw.Length);
            }
        }
    }
}