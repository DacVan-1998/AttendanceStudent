using System.Diagnostics.CodeAnalysis;

#pragma warning disable 8618
#pragma warning disable 1591
namespace AttendanceStudent.Database.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ClamAvConfiguration
    {
        // ReSharper disable once InconsistentNaming
        public string ServerIP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 12700;
    }
}