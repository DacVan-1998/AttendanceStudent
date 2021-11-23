using System.Globalization;
using Microsoft.Extensions.Localization;

namespace AttendanceStudent.Commons.Interfaces
{
    public interface IStringLocalizationService
    {
        LocalizedString this[string key] { get; }
        LocalizedString this[string key, params object[] args] { get; }
        LocalizedString this[string key, CultureInfo cultureInfo, params object[] args] { get; }
    }
}