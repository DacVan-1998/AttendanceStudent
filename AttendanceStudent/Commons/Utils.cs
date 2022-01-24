using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using Column = AttendanceStudent.Commons.Attribute.Column;

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
        
        public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : new()
        {
            bool OnlyColumn(CustomAttributeData y) => y.AttributeType == typeof(Column);

            var columns = typeof(T)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(OnlyColumn))
                .Select(p => new
                {
                    Property = p,
                    Column = p.GetCustomAttributes<Column>().First().ColumnIndex //safe because if where above
                }).ToList();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);


            //Create the collection container
            var collection = rows.Skip(1)
                .Select(row =>
                {
                    var newObject = new T();
                    columns.ForEach(col =>
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        var val = worksheet.Cells[row, col.Column];
                        //If it is numeric it is a double since that is how excel stores all numbers
                        if (val.Value == null)
                        {
                            col.Property.SetValue(newObject, null);
                            return;
                        }

                        if (col.Property.PropertyType == typeof(int))
                        {
                            col.Property.SetValue(newObject, val.GetValue<int>());
                            return;
                        }

                        if (col.Property.PropertyType == typeof(double))
                        {
                            col.Property.SetValue(newObject, val.GetValue<double>());
                            return;
                        }

                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(newObject, val.GetValue<DateTime>());
                            return;
                        }

                        //Its a string
                        col.Property.SetValue(newObject, val.GetValue<string>());
                    });

                    return newObject;
                });


            //Send it back
            return collection;
        }
    }
}