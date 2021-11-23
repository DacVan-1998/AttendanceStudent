using System.Collections.Generic;
using Application.Common.Models;
using AttendanceStudent.Commons.Converters;
using AttendanceStudent.Commons.Interfaces;

namespace AttendanceStudent.Commons.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// To build error with unknown field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        public static IEnumerable<ErrorItem> ToErrors(this string value, IStringLocalizationService localizationService)
        {
            return new[]
            {
                new ErrorItem()
                {
                    Error = localizationService[value].Value,
                    FieldName = LocalizationString.Common.UnknownFieldName
                }
            };
        }

        /// <summary>
        /// To covert base 36 string to base 10
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ConvertBase36ToBase10(this string value)
        {
            return Base.Decode(value);
        }
        
        /// <summary>
        /// To covert base 10 string to base 36
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertBase10ToBase36(this int value)
        {
            return Base.Encode(value);
        }
    }
}